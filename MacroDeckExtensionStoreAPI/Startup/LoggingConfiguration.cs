using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace MacroDeckExtensionStoreAPI.Startup;

public static class LoggingConfiguration
{
    public static void ConfigureSerilog(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Host.UseSerilog((_, _, configuration) =>
            configuration
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Information)
                .WriteTo.Console(theme: AnsiConsoleTheme.Code));
    }
}