using Serilog;

namespace Encryption_service.Logging;

public static class LoggerBuilder 
{
    public static Serilog.ILogger Build(SinkConfigurationDelegate sinkConfiguration) 
    {
        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Information();

        loggerConfiguration = sinkConfiguration.Invoke(loggerConfiguration);

        return loggerConfiguration.CreateLogger();
    }

    public delegate LoggerConfiguration SinkConfigurationDelegate(LoggerConfiguration loggerConfiguration);
}