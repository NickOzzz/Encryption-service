using Encryption_service.Dtos;
using Encryption_service.Events;
using Encryption_service.Helpers;
using LaYumba.Functional;
using Microsoft.AspNetCore.DataProtection;

namespace Encryption_service.Services
{
    public class CrypticService : ICrypticService
    {
        private readonly IDataProtectionProvider crypticProvider;

        public CrypticService(IDataProtectionProvider crypticProvider)
            => this.crypticProvider = crypticProvider;

        public Task<IEncryptionEvent> Encrypt(string message)
            => EncryptAsync(KeyHelpers.GenerateKey(), message).Recover(ToEncryptionFallback);

        public Task<IDecryptionEvent> Decrypt(EncryptedMessageDto messageDto)
            => DecryptAsync(messageDto.encryptedMessage, messageDto.key).Recover(ToDecryptionFallback);

        private Task<IEncryptionEvent> EncryptAsync(string key, string message)
            => crypticProvider.CreateProtector(key)
                .Pipe(protector => protector.Protect(message))
                .Pipe(protectedString => Task.FromResult(new SuccesfullyEncrypted(protectedString, key) as IEncryptionEvent));

        private Task<IDecryptionEvent> DecryptAsync(string encryptedMessage, string key)
            => crypticProvider.CreateProtector(key)
                .Pipe(protector => protector.Unprotect(encryptedMessage))
                .Pipe(unprotectedString => Task.FromResult(new SuccessfullyDecrypted(unprotectedString) as IDecryptionEvent));

        private IEncryptionEvent ToEncryptionFallback(Exception ex)
            => new FailedEncryption(ex.Message);

        private IDecryptionEvent ToDecryptionFallback(Exception ex)
            => new FailedDecryption(ex.Message);
    }
}
