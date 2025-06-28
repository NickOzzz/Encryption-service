using Encryption_service.Dtos;
using Encryption_service.Events;
using LaYumba.Functional;
using Microsoft.AspNetCore.DataProtection;
using static Encryption_service.Helpers.KeyHelpers;

namespace Encryption_service.Services;

public sealed class CrypticService : ICrypticService
{
    private readonly IDataProtectionProvider _crypticProvider;

    public CrypticService(IDataProtectionProvider crypticProvider)
        => _crypticProvider = crypticProvider;

    public Task<IEncryptionEvent> Encrypt(MessageToEncryptDto messageDto)
        => EncryptAsync(messageDto.Message, GenerateKeyIfNullOrEmpty(messageDto.Key)).Recover(ToEncryptionErrorFallback);

    public Task<IDecryptionEvent> Decrypt(EncryptedMessageDto messageDto)
        => ValidateKey(messageDto)
                .Match(
                    Valid: _ => DecryptAsync(messageDto.EncryptedMessage, messageDto.Key),
                    Invalid: errors => Task.FromResult(ToDecryptionErrorFallback(new Exception(errors.FirstOrDefault()?.Message ?? string.Empty))))
                .Recover(ToDecryptionErrorFallback);

    private Task<IEncryptionEvent> EncryptAsync(string message, string key)
        => Task.FromResult(_crypticProvider.CreateProtector(key))
            .Map(protector => protector.Protect(message))
            .Map(protectedString => new SuccessfullyEncrypted(protectedString, key) as IEncryptionEvent);

    private Task<IDecryptionEvent> DecryptAsync(string encryptedMessage, string key)
        => Task.FromResult(_crypticProvider.CreateProtector(key))
            .Map(protector => protector.Unprotect(encryptedMessage))
            .Map(unprotectedString => new SuccessfullyDecrypted(unprotectedString) as IDecryptionEvent);

    private static string GenerateKeyIfNullOrEmpty(string? key)
        => KeyIsNullOrEmpty(key)
                ? GenerateKey()
                : key!;

    private static Validation<EncryptedMessageDto> ValidateKey(EncryptedMessageDto messageDto)
        => !KeyIsNullOrEmpty(messageDto.Key)
                ? F.Valid(messageDto) 
                : F.Invalid("Key cannot be null, empty or whitespace");

    private static bool KeyIsNullOrEmpty(string? key)
        => string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key);

    private static IEncryptionEvent ToEncryptionErrorFallback(Exception ex)
        => new FailedEncryption(ex.Message);

    private static IDecryptionEvent ToDecryptionErrorFallback(Exception ex)
        => new FailedDecryption(ex.Message);
}
