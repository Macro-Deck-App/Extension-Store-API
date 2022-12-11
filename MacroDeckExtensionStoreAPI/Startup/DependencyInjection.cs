using System.Net;
using MacroDeckExtensionStoreLibrary.Data;
using MacroDeckExtensionStoreLibrary.Interfaces;
using MacroDeckExtensionStoreLibrary.Parsers;
using MacroDeckExtensionStoreLibrary.Repositories;
using MacroDeckExtensionStoreLibrary.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
        var apiToken = builder.Configuration["APIToken"];
        builder.Services.AddDbContext<ExtensionStoreDbContext>(options => 
            options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "API Key Authorization header",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
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