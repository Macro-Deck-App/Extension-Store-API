using System.Net.Http.Json;
using ExtensionStoreAPI.Core.Helper;
using ExtensionStoreAPI.Core.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ExtensionStoreAPI.Core.Configuration;

public partial class ExtensionStoreApiConfig
{
     private static readonly ILogger Logger = Log.ForContext(typeof(ExtensionStoreApiConfig));

    private static IConfiguration? Configuration { get; set; }

    private const string ConfigPath = "Config/config.json";

    private static int CurrentConfigVersion { get; set; } = -1;
    
    public static async ValueTask Initialize()
    {
        Logger.Information("Loading config from {ConfigPath}...", ConfigPath);

        if (EnvironmentHelper.IsStagingOrProduction)
        {
            Logger.Information(
                "Service was started in staging or production environment. Will download config from ConfigService");
            await UpdateConfig(ConfigPath, CancellationToken.None);
            StartUpdateLoop();
        }

        var configBuilder = new ConfigurationBuilder();
        var config = LoadConfigurationFile(ConfigPath);
        configBuilder.Add(config);
        
        Configuration = configBuilder.Build();
        Logger.Information("Configuration loaded");
    }

    private static JsonConfigurationSource LoadConfigurationFile(string configPath)
    {
        return new JsonConfigurationSource
        {
            Path = configPath,
            ReloadOnChange = true,
            ReloadDelay = 2000,
            Optional = false
        };
    }

    private static string GetString(string key)
    {
        if (Configuration == null)
        {
            throw new InvalidOperationException("Configuration is not loaded");
        }
        var value = Configuration.GetValue<string>(key);
        if (value != null)
        {
            return value;
        }

        Logger.Fatal("Cannot find value in config for {Key}", key);
        return string.Empty;
    }
    
    private static void StartUpdateLoop()
    {
        Task.Run(async () => await UpdateLoop(CancellationToken.None));
    }

    private static async Task UpdateLoop(CancellationToken cancellationToken)
    {
        do
        {
            await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            var latestConfigVersion = await GetLatestConfigVersion();
            if (latestConfigVersion == CurrentConfigVersion)
            {
                continue;
            }
            
            await UpdateConfig(ConfigPath, cancellationToken);
        } while (!cancellationToken.IsCancellationRequested);
    }

    private static async Task<int> GetLatestConfigVersion()
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("x-config-name", new[] { EnvironmentHelper.ConfigServiceConfigName });
        httpClient.DefaultRequestHeaders.Add("x-config-access-token",
            new[] { EnvironmentHelper.ConfigServiceAuthToken });

        var response = await httpClient.GetAsync($"{EnvironmentHelper.ConfigServiceUrl}/config/version");
        var resultString = await response.Content.ReadAsStringAsync();
        return int.TryParse(resultString, out var result) ? result : 0;
    }

    private static async ValueTask UpdateConfig(string configPath, CancellationToken cancellationToken)
    {
        try
        {
            var encodedConfig = await DownloadFromConfigService();
            var configJson = Base64Utils.Base64Decode(encodedConfig.ConfigBase64 ?? string.Empty);
            
            await File.WriteAllTextAsync(configPath, configJson, cancellationToken);
            CurrentConfigVersion = encodedConfig.Version;
            Logger.Information("Config version {Version} downloaded from ConfigService", encodedConfig.Version);
        }
        catch (Exception ex)
        {
            Logger.Fatal(ex, "Could not update config");
        }
    }

    private static async ValueTask<EncodedConfig> DownloadFromConfigService()
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("x-config-name", new []{ EnvironmentHelper.ConfigServiceConfigName });
        httpClient.DefaultRequestHeaders.Add("x-config-access-token",
            new[] { EnvironmentHelper.ConfigServiceAuthToken });

        return await httpClient.GetFromJsonAsync<EncodedConfig>(
            $"{EnvironmentHelper.ConfigServiceUrl}/config/encoded")
               ?? throw new InvalidOperationException("Could not download config");
    }
}