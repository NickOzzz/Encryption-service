using Microsoft.AspNetCore.Mvc;

namespace Encryption_service.Controllers
{
    [ApiController]
    public class ErrorController : Controller
    {
        [HttpGet("error")]
        public IActionResult Error()
            => new ObjectResult(new { Error = "Unknown error occured. Please reach out to admin" })
            {
                StatusCode = 500
            };
    }
}
