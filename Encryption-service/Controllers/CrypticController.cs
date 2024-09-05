using Encryption_service.Dtos;
using Encryption_service.Services;
using LaYumba.Functional;
using Microsoft.AspNetCore.Mvc;
using static Encryption_service.Controllers.CrypticControllerResults;

namespace Encryption_service.Controllers
{
    [ApiController]
    [Route("api")]
    public class CrypticController : Controller
    {
        private readonly ICrypticService crypticService;

        public CrypticController(ICrypticService crypticService)
            => this.crypticService = crypticService;

        [HttpPost("encrypt")]
        public Task<IActionResult> EncryptMessage([FromBody] MessageToEncryptDto message)
            => crypticService.Encrypt(message.message).Map(CreateEncryptionResult());

        [HttpPost("decrypt")]
        public Task<IActionResult> DecryptMessage([FromBody] EncryptedMessageDto encryptedMessage)
            => crypticService.Decrypt(encryptedMessage).Map(CreateDecryptionResult());
    }
}
