using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ExtensionStoreAPI.Startup;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerOptions>();
    }

    public static void ConfigureSwagger(this WebApplication app)
    { 
        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
            {
                options.SwaggerEndpoint($"{description.GroupName}/swagger.json",
                    description.GroupName);
            }
        });
    }
    
}