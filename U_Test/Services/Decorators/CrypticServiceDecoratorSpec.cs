using Encryption_service.Dtos;
using Encryption_service.Events;
using Encryption_service.Services;
using Encryption_service.Services.Decorators;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U_Test.Services.Decorators;

public class CrypticServiceDecoratorSpec
{
    [Fact]
    public async Task EncryptReturnsSuccess() 
    {
        var mockService = new Mock<ICrypticService>();
        mockService.Setup(x => x.Encrypt(It.IsNotNull<string>()))
            .ReturnsAsync(new SuccessfullyEncrypted(string.Empty, string.Empty));

        var serviceDecorator = new CrypticServiceDecorator(mockService.Object, Mock.Of<Serilog.ILogger>());

        var result = await serviceDecorator.Encrypt(string.Empty);

        result.Should().NotBeNull();
        result.Should().BeOfType<SuccessfullyEncrypted>();

        mockService.Verify(x => x.Encrypt(It.IsNotNull<string>()), Times.Once());
    }

    [Fact]
    public async Task DecryptReturnsSuccess()
    {
        var mockService = new Mock<ICrypticService>();
        mockService.Setup(x => x.Decrypt(It.IsNotNull<EncryptedMessageDto>()))
            .ReturnsAsync(new SuccessfullyDecrypted(string.Empty));

        var serviceDecorator = new CrypticServiceDecorator(mockService.Object, Mock.Of<Serilog.ILogger>());

        var result = await serviceDecorator.Decrypt(new EncryptedMessageDto(string.Empty, string.Empty));

        result.Should().NotBeNull();
        result.Should().BeOfType<SuccessfullyDecrypted>();

        mockService.Verify(x => x.Decrypt(It.IsNotNull<EncryptedMessageDto>()), Times.Once());
    }
}