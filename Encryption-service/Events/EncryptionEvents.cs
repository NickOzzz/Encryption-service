namespace Encryption_service.Events;

public interface IEncryptionEvent { }
public record SuccessfullyEncrypted(string EncryptedMessage, string Key) : IEncryptionEvent;
public record FailedEncryption(string Error) : IEncryptionEvent;
