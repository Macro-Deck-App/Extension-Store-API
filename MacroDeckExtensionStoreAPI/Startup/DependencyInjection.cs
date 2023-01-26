using System.Text.Json.Serialization;
using MacroDeckExtensionStoreAPI.Config;
using MacroDeckExtensionStoreLibrary;
using MacroDeckExtensionStoreLibrary.Automapper;
using MacroDeckExtensionStoreLibrary.DataAccess;
using MacroDeckExtensionStoreLibrary.DataAccess.Repositories;
using MacroDeckExtensionStoreLibrary.DataAccess.RepositoryInterfaces;
using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.ManagerInterfaces;
using MacroDeckExtensionStoreLibrary.Managers;
using MacroDeckExtensionStoreLibrary.Parsers;
using MacroDeckExtensionStoreLibrary.Services;
using Microsoft.EntityFrameworkCore;

namespace MacroDeckExtensionStoreAPI.Startup;

public static class DependencyInjection
{
    public static async Task ConfigureAsync(this WebApplicationBuilder builder)
    {
        builder.ConfigureSerilog();
        Paths.EnsureDirectoriesCreated();
        
        var appConfig = await AppConfig.LoadAsync(Paths.AppConfigPath);
        builder.Services.AddSingleton(appConfig);
        
        builder.Services.AddAutoMapper(typeof(ExtensionProfile));
        builder.Services.AddAutoMapper(typeof(ExtensionFileProfile));
        
        var databaseConfig = await DatabaseConfig.LoadAsync(Paths.DatabaseConfigPath);
        var psqlConnectionStr = databaseConfig.ToConnectionString();
        builder.Services.AddDbContext<ExtensionStoreDbContext>(options =>
            options.UseNpgsql(psqlConnectionStr));
        
        builder.Services.AddScoped<IExtensionRepository, ExtensionRepository>();
        builder.Services.AddScoped<IExtensionFileRepository, ExtensionFileRepository>();
        builder.Services.AddScoped<IExtensionManager, ExtensionManager>();
        builder.Services.AddScoped<IExtensionFileManager, ExtensionFileManager>();

        builder.Services.AddScoped<IGitHubRepositoryLicenseUrlParser, GitHubRepositoryLicenseUrlParser>();
        builder.Services.AddScoped<IGitHubRepositoryService, GitHubRepositoryService>();
        builder.Services.AddScoped<HttpClient>();
        
        builder.Services.AddSwagger();
        builder.Services.AddControllers()
            .AddJsonOptions(opts =>
            {
                var enumConverter = new JsonStringEnumConverter();
                opts.JsonSerializerOptions.Converters.Add(enumConverter);
                opts.JsonSerializerOptions.AllowTrailingCommas = true;
            });
    }
}