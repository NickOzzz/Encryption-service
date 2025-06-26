using Encryption_service.Dtos;
using Encryption_service.Events;
using Encryption_service.Helpers;
using LaYumba.Functional;
using Microsoft.AspNetCore.DataProtection;

namespace Encryption_service.Services;

public sealed class CrypticService : ICrypticService
{
    private readonly IDataProtectionProvider _crypticProvider;

    public CrypticService(IDataProtectionProvider crypticProvider)
        => _crypticProvider = crypticProvider;

    public Task<IEncryptionEvent> Encrypt(string message)
        => EncryptAsync(KeyHelpers.GenerateKey(), message).Recover(ToEncryptionFallback);

    public Task<IDecryptionEvent> Decrypt(EncryptedMessageDto messageDto)
        => DecryptAsync(messageDto.EncryptedMessage, messageDto.Key).Recover(ToDecryptionFallback);

    private Task<IEncryptionEvent> EncryptAsync(string key, string message)
        => Task.FromResult(_crypticProvider.CreateProtector(key))
            .Map(protector => protector.Protect(message))
            .Map(protectedString => new SuccessfullyEncrypted(protectedString, key) as IEncryptionEvent);

    private Task<IDecryptionEvent> DecryptAsync(string encryptedMessage, string key)
        => Task.FromResult(_crypticProvider.CreateProtector(key))
            .Map(protector => protector.Unprotect(encryptedMessage))
            .Map(unprotectedString => new SuccessfullyDecrypted(unprotectedString) as IDecryptionEvent);

    private static IEncryptionEvent ToEncryptionFallback(Exception ex)
        => new FailedEncryption(ex.Message);

    private static IDecryptionEvent ToDecryptionFallback(Exception ex)
        => new FailedDecryption(ex.Message);
}
