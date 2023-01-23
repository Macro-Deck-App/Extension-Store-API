using MacroDeckExtensionStoreAPI.Config;
using MacroDeckExtensionStoreLibrary.DataAccess;
using MacroDeckExtensionStoreLibrary.DataAccess.Entities;
using MacroDeckExtensionStoreLibrary.DataAccess.Repositories;
using MacroDeckExtensionStoreLibrary.DataAccess.RepositoryInterfaces;
using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.ManagerInterfaces;
using MacroDeckExtensionStoreLibrary.Managers;
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
        builder.Services.AddSingleton(appConfig);
        
        var databaseConfig = await DatabaseConfig.LoadAsync(Paths.DatabaseConfigPath);
        var psqlConnectionStr = databaseConfig.ToConnectionString();
        builder.Services.AddDbContext<ExtensionStoreDbContext>(options =>
            options.UseNpgsql(psqlConnectionStr));
        
        builder.Services.AddScoped<IExtensionRepository, ExtensionRepository>();
        builder.Services.AddScoped<IExtensionManager, ExtensionManager>();
        builder.Services.AddScoped<IExtensionFileRepository, ExtensionFileRepository>();

        builder.Services.AddScoped<IGitHubRepositoryLicenseUrlParser, GitHubRepositoryLicenseUrlParser>();
        builder.Services.AddScoped<IGitHubRepositoryService, GitHubRepositoryService>();
        builder.Services.AddScoped<HttpClient>();
        
        builder.Services.AddSwagger();
        builder.Services.AddControllers();
    }
}