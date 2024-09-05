using Encryption_service.Dtos;
using Encryption_service.Events;

namespace Encryption_service.Services
{
    public interface ICrypticService
    {
        Task<IEncryptionEvent> Encrypt(string message);
        Task<IDecryptionEvent> Decrypt(EncryptedMessageDto messageDto);
    }
}
