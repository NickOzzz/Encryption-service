using Encryption_service.Dtos;
using Encryption_service.Events;

namespace Encryption_service.Services.Decorators;

public class CrypticServiceDecorator : ICrypticService 
{
    private readonly ICrypticService _next;
    private readonly Serilog.ILogger _logger;

    public CrypticServiceDecorator(ICrypticService next, Serilog.ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task<IEncryptionEvent> Encrypt(MessageToEncryptDto messageDto) 
    {
        var startingTime = DateTime.UtcNow;
        _logger.Information("Starting {methodName} at: {startingTime}", nameof(Encrypt), startingTime);

        var result = await _next.Encrypt(messageDto);

        var usedCustomKey = true;
        var key = messageDto.Key;
        if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            usedCustomKey = false;

        var timeElapsed = DateTime.UtcNow - startingTime;
        _logger.Information("Finishing {methodName} with elapsed time: {timeElapsed} sec. Used custom encryption key: {usedCustomKey}", nameof(Encrypt), timeElapsed.TotalSeconds, usedCustomKey);

        return result;
    }

    public async Task<IDecryptionEvent> Decrypt(EncryptedMessageDto messageDto)
    {
        var startingTime = DateTime.UtcNow;
        _logger.Information("Starting {methodName} at: {startingTime}", nameof(Decrypt), startingTime);

        var result = await _next.Decrypt(messageDto);

        var timeElapsed = DateTime.UtcNow - startingTime;
        _logger.Information("Finishing {methodName} with elapsed time: {timeElapsed} sec.", nameof(Decrypt), timeElapsed.TotalSeconds);

        return result;
    }
}