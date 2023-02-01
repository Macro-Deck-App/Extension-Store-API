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

        builder.Services.AddHttpClient();
        
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

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwagger();
        builder.Services.AddControllers()
            .AddJsonOptions(opt =>
            {
                var enumConverter = new JsonStringEnumConverter();
                opt.JsonSerializerOptions.Converters.Add(enumConverter);
                opt.JsonSerializerOptions.AllowTrailingCommas = true;
            });
        builder.Services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(2,0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
        });
        builder.Services.AddVersionedApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });
    }
}