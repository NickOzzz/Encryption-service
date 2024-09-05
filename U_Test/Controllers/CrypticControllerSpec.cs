using Encryption_service.Controllers;
using Encryption_service.Dtos;
using Encryption_service.Events;
using Encryption_service.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace U_Test.Controllers
{
    public class CrypticControllerSpec
    {
        [Fact]
        public async Task EncryptReturnsSuccess()
        {
            var encryptionService = new Mock<ICrypticService>();
            encryptionService.Setup(service => service.Encrypt(It.IsAny<string>())).Returns(Task.FromResult(new SuccessfullyEncrypted("encrypted", "key") as IEncryptionEvent));

            var controller = new CrypticController(encryptionService.Object);
            var result = await controller.EncryptMessage(new MessageToEncryptDto("testMessage"));

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task EncryptReturnsFailure()
        {
            var encryptionService = new Mock<ICrypticService>();
            encryptionService.Setup(service => service.Encrypt(It.IsAny<string>())).Returns(Task.FromResult(new FailedEncryption("error") as IEncryptionEvent));

            var controller = new CrypticController(encryptionService.Object);
            var result = await controller.EncryptMessage(new MessageToEncryptDto("testMessage"));

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DecryptReturnsSuccess()
        {
            var encryptionService = new Mock<ICrypticService>();
            encryptionService.Setup(service => service.Decrypt(It.IsAny<EncryptedMessageDto>())).Returns(Task.FromResult(new SuccessfullyDecrypted("decrypted") as IDecryptionEvent));

            var controller = new CrypticController(encryptionService.Object);
            var result = await controller.DecryptMessage(new EncryptedMessageDto("encrypted", "key"));

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DecryptReturnsFailure()
        {
            var encryptionService = new Mock<ICrypticService>();
            encryptionService.Setup(service => service.Decrypt(It.IsAny<EncryptedMessageDto>())).Returns(Task.FromResult(new FailedDecryption("error") as IDecryptionEvent));

            var controller = new CrypticController(encryptionService.Object);
            var result = await controller.DecryptMessage(new EncryptedMessageDto("encrypted", "key"));

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}