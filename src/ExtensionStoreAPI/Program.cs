using ExtensionStoreAPI.Core.Configuration;
using ExtensionStoreAPI.Core.Helper;
using ExtensionStoreAPI.Extensions;
using ExtensionStoreAPI.Setup;
using Serilog;

namespace ExtensionStoreAPI;

public static class Program
{
    public static async Task Main(string[] args)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

        await ExtensionStoreApiConfig.Initialize();
        
        var app = Host.CreateDefaultBuilder(args)
            .ConfigureSerilog()
            .ConfigureWebHostDefaults(hostBuilder =>
            {
                hostBuilder.UseStartup<Startup>();
                hostBuilder.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(EnvironmentHelper.HostingPort);
                });
            }).Build();

        await app.MigrateDatabaseAsync();
        await app.RunAsync();
    }
    
    private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Log.Logger.Fatal(e.ExceptionObject as Exception,
            "Unhandled exception {Terminating}",
            e.IsTerminating
                ? "Terminating"
                : "Not terminating");
    }
}