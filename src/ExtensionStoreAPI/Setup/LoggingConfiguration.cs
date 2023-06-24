using System.Diagnostics;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace ExtensionStoreAPI.Setup;

public static class LoggingConfiguration
{
    public static IHostBuilder ConfigureSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((_, _, configuration) =>
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
                .WriteTo.Console(theme: AnsiConsoleTheme.Code));
    }
}