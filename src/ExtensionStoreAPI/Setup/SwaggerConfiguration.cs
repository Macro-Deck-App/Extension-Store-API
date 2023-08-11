using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ExtensionStoreAPI.Setup;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerOptions>();
    }

    public static void ConfigureSwagger(this IApplicationBuilder app)
    {
        var apiVersionDescriptionProvider =
            app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
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