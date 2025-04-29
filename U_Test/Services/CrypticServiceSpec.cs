using Encryption_service.Events;
using Encryption_service.Services;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;

namespace U_Test.Services
{
    public class CrypticServiceSpec
    {
        [Fact]
        public async Task EncryptReturnsSuccess()
        {
            var service = new CrypticService(CreateProtectionProvider());

            var result = await service.Encrypt("someMessage");

            result.Should().BeOfType<SuccessfullyEncrypted>();
            (result as SuccessfullyEncrypted).EncryptedMessage.Should().NotBeNullOrEmpty();
            (result as SuccessfullyEncrypted).Key.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task EncryptReturnsFailure()
        {
            var service = new CrypticService(CreateProtectionProvider());

            var result = await service.Encrypt(null);

            result.Should().BeOfType<FailedEncryption>();
            (result as FailedEncryption).Error.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task DecryptReturnsSuccess()
        {
            var service = new CrypticService(CreateProtectionProvider());

            var encryptedMessage = await service.Encrypt("someMessage") as SuccessfullyEncrypted;
            var result = await service.Decrypt(new(encryptedMessage.EncryptedMessage, encryptedMessage.Key));

            result.Should().BeOfType<SuccessfullyDecrypted>();
            (result as SuccessfullyDecrypted).Message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task DecryptReturnsFailure()
        {
            var service = new CrypticService(CreateProtectionProvider());

            var result = await service.Decrypt(new("encryptedMessage", "key"));

            result.Should().BeOfType<FailedDecryption>();
            (result as FailedDecryption).Error.Should().NotBeNullOrEmpty();
        }

        private IDataProtectionProvider CreateProtectionProvider()
            => DataProtectionProvider.Create(Directory.GetCurrentDirectory());
    }
}
