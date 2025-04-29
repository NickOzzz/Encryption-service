using Microsoft.AspNetCore.Mvc;
using static Encryption_service.Controllers.CrypticControllerResults;

namespace Encryption_service.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
public class ErrorController : Controller
{
    [HttpGet("error")]
    public IActionResult Error()
        => GenerateUnknownError();
}
