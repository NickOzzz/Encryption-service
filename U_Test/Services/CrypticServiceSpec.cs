using Encryption_service.Dtos;
using Encryption_service.Events;
using Encryption_service.Services;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;

namespace U_Test.Services;

public class CrypticServiceSpec
{
    private readonly ICrypticService _crypticService;

    public CrypticServiceSpec()
        => _crypticService = new CrypticService(CreateProtectionProvider());

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task EncryptReturnsSuccessWithUnspecifiedKey(string? key)
    {
        var messageToEncrypt = new MessageToEncryptDto("someMessage", key);

        var result = await _crypticService.Encrypt(messageToEncrypt);

        result.Should().BeOfType<SuccessfullyEncrypted>();
        (result as SuccessfullyEncrypted)!.EncryptedMessage.Should().NotBeNullOrEmpty();
        (result as SuccessfullyEncrypted)!.Key.Should().NotBeNullOrEmpty();
        (result as SuccessfullyEncrypted)!.Key.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task EncryptReturnsSuccessWithSpecifiedKey()
    {
        var messageToEncrypt = new MessageToEncryptDto("someMessage", "testKey");

        var result = await _crypticService.Encrypt(messageToEncrypt);

        result.Should().BeOfType<SuccessfullyEncrypted>();
        (result as SuccessfullyEncrypted)!.EncryptedMessage.Should().NotBeNullOrEmpty();
        (result as SuccessfullyEncrypted)!.Key.Should().Be(messageToEncrypt.Key);
    }

    [Fact]
    public async Task EncryptReturnsFailure()
    {
        var messageToEncrypt = new MessageToEncryptDto(null!, "testKey");

        var result = await _crypticService.Encrypt(messageToEncrypt);

        result.Should().BeOfType<FailedEncryption>();
        (result as FailedEncryption)!.Error.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("testKey")]
    public async Task DecryptReturnsSuccess(string? key)
    {
        var messageToEncrypt = new MessageToEncryptDto("someMessage", key);

        var encryptedMessage = await _crypticService.Encrypt(messageToEncrypt) as SuccessfullyEncrypted;

        encryptedMessage.Should().NotBeNull();

        var encryptedMessageDto = new EncryptedMessageDto(encryptedMessage!.EncryptedMessage, encryptedMessage.Key);

        var result = await _crypticService.Decrypt(encryptedMessageDto);

        result.Should().BeOfType<SuccessfullyDecrypted>();
        (result as SuccessfullyDecrypted)!.Message.Should().NotBeNullOrEmpty();
        (result as SuccessfullyDecrypted)!.Message.Should().Be(messageToEncrypt.Message);
    }

    [Theory]
    [InlineData("encryptedMessage", "key")]
    [InlineData("", "")]
    [InlineData(null, null)]
    public async Task DecryptReturnsFailure(string? message, string? key)
    {
        var encryptedMessageDto = new EncryptedMessageDto(message!, key!);

        var result = await _crypticService.Decrypt(encryptedMessageDto);

        result.Should().BeOfType<FailedDecryption>();
        (result as FailedDecryption)!.Error.Should().NotBeNullOrEmpty();
    }

    private IDataProtectionProvider CreateProtectionProvider()
        => DataProtectionProvider.Create(Directory.GetCurrentDirectory());
}
