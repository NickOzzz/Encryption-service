using Encryption_service.Dtos;
using Encryption_service.Events;
using Encryption_service.Helpers;
using LaYumba.Functional;
using Microsoft.AspNetCore.DataProtection;

namespace Encryption_service.Services
{
    public sealed class CrypticService : ICrypticService
    {
        private readonly IDataProtectionProvider crypticProvider;

        public CrypticService(IDataProtectionProvider crypticProvider)
            => this.crypticProvider = crypticProvider;

        public Task<IEncryptionEvent> Encrypt(string message)
            => EncryptAsync(KeyHelpers.GenerateKey(), message).Recover(ToEncryptionFallback);

        public Task<IDecryptionEvent> Decrypt(EncryptedMessageDto messageDto)
            => DecryptAsync(messageDto.encryptedMessage, messageDto.key).Recover(ToDecryptionFallback);

        private Task<IEncryptionEvent> EncryptAsync(string key, string message)
            => Task.FromResult(crypticProvider.CreateProtector(key))
                .Map(protector => protector.Protect(message))
                .Bind(protectedString => Task.FromResult(new SuccessfullyEncrypted(protectedString, key) as IEncryptionEvent));

        private Task<IDecryptionEvent> DecryptAsync(string encryptedMessage, string key)
            => Task.FromResult(crypticProvider.CreateProtector(key))
                .Map(protector => protector.Unprotect(encryptedMessage))
                .Bind(unprotectedString => Task.FromResult(new SuccessfullyDecrypted(unprotectedString) as IDecryptionEvent));

        private IEncryptionEvent ToEncryptionFallback(Exception ex)
            => new FailedEncryption(ex.Message);

        private IDecryptionEvent ToDecryptionFallback(Exception ex)
            => new FailedDecryption(ex.Message);
    }
}
