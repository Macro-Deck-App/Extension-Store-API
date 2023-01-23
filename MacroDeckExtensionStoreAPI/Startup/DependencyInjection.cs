using MacroDeckExtensionStoreAPI.Config;
using MacroDeckExtensionStoreLibrary.DataAccess;
using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.Parsers;
using MacroDeckExtensionStoreLibrary.Repositories;
using MacroDeckExtensionStoreLibrary.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace MacroDeckExtensionStoreAPI.Startup;

public static class DependencyInjection
{
    public static async Task ConfigureAsync(this WebApplicationBuilder builder)
    {
        Paths.EnsureDirectoriesCreated();
        var dataDirectory = Paths.DataDirectory;
        var appConfig = await AppConfig.LoadAsync(Paths.AppConfigPath);
        var databaseConfig = await DatabaseConfig.LoadAsync(Paths.DatabaseConfigPath);
        var psqlConnectionStr = databaseConfig.ToConnectionString();

        builder.Services.AddDbContext<ExtensionStoreDbContext>(options =>
            options.UseNpgsql(psqlConnectionStr));
        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services.AddScoped<IExtensionsRepository, ExtensionsDbRepository>();
        builder.Services.AddScoped<IExtensionsFilesRepository, ExtensionsFilesFileRepository>(x =>
            new ExtensionsFilesFileRepository(dataDirectory));
        builder.Services.AddScoped<IGitHubRepositoryLicenseUrlParser, GitHubRepositoryLicenseUrlParser>();
        builder.Services.AddScoped<IGitHubRepositoryService, GitHubRepositoryService>();
        builder.Services.AddScoped<HttpClient>();
        builder.Services.AddSingleton(appConfig);
        
        builder.Services.AddControllers();
    }
}