using Serilog;
using Serilog.Configuration;

namespace ExtensionStoreAPI.Logging;

public static class CustomSinkExtensions
{
    public static LoggerConfiguration MacroBotSink(
        this LoggerSinkConfiguration loggerConfiguration,
        IServiceProvider serviceProvider)
    {
        return loggerConfiguration.Sink(new MacroBotSink(serviceProvider));
    }
}