using Encryption_service.Events;
using Microsoft.AspNetCore.Mvc;

namespace Encryption_service.Controllers
{
    public static class CrypticControllerResults
    {
        public static Func<IEncryptionEvent, IActionResult> CreateEncryptionResult()
            => encryptionEvent => encryptionEvent switch
            {
                SuccesfullyEncrypted success => new OkObjectResult(success),
                FailedEncryption failed => new BadRequestObjectResult(failed),
                _ => GenerateUnknownError()
            };

        public static Func<IDecryptionEvent, IActionResult> CreateDecryptionResult()
           => encryptionEvent => encryptionEvent switch
           {
               SuccessfullyDecrypted success => new OkObjectResult(success),
               FailedDecryption failed => new BadRequestObjectResult(failed),
               _ => GenerateUnknownError()
           };

        private static IActionResult GenerateUnknownError()
            => new ObjectResult(new { error = "Unknown error. Please reach out to admin." })
            { 
                StatusCode = 500
            };
    }
}
