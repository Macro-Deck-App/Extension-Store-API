using System.Net;
using MacroDeckExtensionStoreLibrary.Data;
using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.Parsers;
using MacroDeckExtensionStoreLibrary.Repositories;
using MacroDeckExtensionStoreLibrary.Services;
using Microsoft.EntityFrameworkCore;

namespace MacroDeckExtensionStoreAPI.Startup;

public static class DependencyInjection
{
    public static void ConfigureAppSettings(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
            .AddEnvironmentVariables();
    }
    
    public static void Configure(this WebApplicationBuilder builder)
    {
        var mySqlConnectionStr = builder.Configuration.GetConnectionString("Default");
        var dataPath = builder.Configuration["Directories:Data"];
        builder.Services.AddDbContext<ExtensionStoreDbContext>(options => 
            options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IExtensionsRepository, ExtensionsDbRepository>();
        builder.Services.AddScoped<IExtensionsFilesRepository, ExtensionsFilesFileRepository>(x => new ExtensionsFilesFileRepository(dataPath));
        builder.Services.AddScoped<IGitHubRepositoryLicenseUrlParser, GitHubRepositoryLicenseUrlParser>();
        builder.Services.AddScoped<IGitHubRepositoryService, GitHubRepositoryService>();
        builder.Services.AddScoped<HttpClient>();
        
        // services.AddTransient<ICustomerRepository, CustomerDbRepository>();
        // services.AddTransient<IInvoiceRepository, InvoiceDbRepository>();
        // services.AddTransient<IInvoiceGenerator, InvoiceGenerator>();
        
        
        builder.Services.AddControllers();
    }
}