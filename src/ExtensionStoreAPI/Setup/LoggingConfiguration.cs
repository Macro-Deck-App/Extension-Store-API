using System.Diagnostics;
using ExtensionStoreAPI.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace ExtensionStoreAPI.Setup;

public static class LoggingConfiguration
{
    public static IHostBuilder ConfigureSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((_, serviceProvider, configuration) =>
            configuration
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft",
                    Debugger.IsAttached
                        ? LogEventLevel.Debug
                        : LogEventLevel.Information)
                .MinimumLevel.Override("System.Net.Http.HttpClient",
                    Debugger.IsAttached
                        ? LogEventLevel.Debug
                        : LogEventLevel.Information)
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .WriteTo.Logger(lc =>
                    lc.Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Warning).WriteTo
                        .MacroBotSink(serviceProvider)));
    }
}