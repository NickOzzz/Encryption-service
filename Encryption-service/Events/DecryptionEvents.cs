namespace Encryption_service.Events;

public interface IDecryptionEvent { }
public record SuccessfullyDecrypted(string Message) : IDecryptionEvent;
public record FailedDecryption(string Error) : IDecryptionEvent;
