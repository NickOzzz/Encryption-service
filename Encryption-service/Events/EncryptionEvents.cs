namespace Encryption_service.Events
{
    public interface IEncryptionEvent { }
    public record SuccessfullyEncrypted(string encryptedMessage, string key) : IEncryptionEvent;
    public record FailedEncryption(string error) : IEncryptionEvent;
}
