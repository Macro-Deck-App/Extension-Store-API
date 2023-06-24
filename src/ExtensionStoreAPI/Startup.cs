using System.Text.Json.Serialization;
using ExtensionStoreAPI.Core;
using ExtensionStoreAPI.Core.DataAccess;
using ExtensionStoreAPI.Core.Interfaces;
using ExtensionStoreAPI.Core.Parsers;
using ExtensionStoreAPI.Core.Services;
using ExtensionStoreAPI.Middleware;
using ExtensionStoreAPI.Setup;

namespace ExtensionStoreAPI;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddAutoMapper(typeof(Startup).Assembly);
        services.RegisterClassesEndsWithAsScoped("Repository");
        services.RegisterClassesEndsWithAsScoped("Manager");
        services.AddDbContext<ExtensionStoreDbContext>();
        services.AddScoped<IGitHubRepositoryLicenseUrlParser, GitHubRepositoryLicenseUrlParser>();
        services.AddScoped<IGitHubRepositoryService, GitHubRepositoryService>();
        services.AddEndpointsApiExplorer();
        services.AddSwagger();
        services.AddControllers()
            .AddJsonOptions(opt =>
            {
                var enumConverter = new JsonStringEnumConverter();
                opt.JsonSerializerOptions.Converters.Add(enumConverter);
                opt.JsonSerializerOptions.AllowTrailingCommas = true;
            });
        services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(2,0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
        });
        services.AddVersionedApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        Paths.EnsureDirectoriesCreated();
        app.UseCors("AllowAny");
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseFileServer();
        app.UseWebSockets(new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromMinutes(2)
        });
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.ConfigureSwagger();
    }
}