using Encryption_service.Dtos;
using Encryption_service.Services;
using LaYumba.Functional;
using Microsoft.AspNetCore.Mvc;
using static Encryption_service.Controllers.CrypticControllerResults;

namespace Encryption_service.Controllers;

[ApiController]
[Route("api")]
public class CrypticController : Controller
{
    private readonly ICrypticService _crypticService;

    public CrypticController(ICrypticService crypticService)
        => _crypticService = crypticService;

    /// <remarks>If key is null, empty or whitespace then random one will be generated!</remarks>
    [HttpPost("encrypt")]
    public Task<IActionResult> EncryptMessage([FromBody] MessageToEncryptDto message)
        => _crypticService.Encrypt(message).Map(CreateEncryptionResult());

    [HttpPost("decrypt")]
    public Task<IActionResult> DecryptMessage([FromBody] EncryptedMessageDto encryptedMessage)
        => _crypticService.Decrypt(encryptedMessage).Map(CreateDecryptionResult());
}
