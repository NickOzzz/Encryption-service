using Serilog;

namespace Encryption_service.Logging;

public static class LoggerBuilder 
{
    public static Serilog.ILogger Build(Func<LoggerConfiguration, LoggerConfiguration> sinkConfiguration) 
    {
        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Information();

        loggerConfiguration = sinkConfiguration.Invoke(loggerConfiguration);

        return loggerConfiguration.CreateLogger();
    }
}