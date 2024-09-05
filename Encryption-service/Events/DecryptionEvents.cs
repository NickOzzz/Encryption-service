namespace Encryption_service.Events
{
    public interface IDecryptionEvent { }
    public record SuccessfullyDecrypted(string message) : IDecryptionEvent;
    public record FailedDecryption(string error) : IDecryptionEvent;
}
