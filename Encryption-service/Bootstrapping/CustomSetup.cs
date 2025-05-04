using Encryption_service.Logging;
using Encryption_service.Services;
using Encryption_service.Services.Decorators;
using Microsoft.AspNetCore.DataProtection;
using Serilog;

namespace Encryption_service.Bootstrapping;

public static class CustomSetup
{
    public static IServiceCollection AddCustomSetup(this IServiceCollection services)
        => services
             .AddTransient<ICrypticService, CrypticService>()
             .AddTransient(_ => DataProtectionProvider.Create(Directory.GetCurrentDirectory()))
             .AddSingleton(_ => LoggerBuilder.Build(SetupLoggerSink))
             .AddDecorators();

    private static IServiceCollection AddDecorators(this IServiceCollection services)
        => services.Decorate<ICrypticService, CrypticServiceDecorator>();

    private static LoggerConfiguration SetupLoggerSink(LoggerConfiguration loggerConfiguration)
        => loggerConfiguration.WriteTo.Console();
}
