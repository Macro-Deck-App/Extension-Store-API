namespace MacroDeckExtensionStoreAPI.Startup;

public static class SwaggerConfiguration
{

    public static void ConfigureSwagger(this WebApplication app)
    { 
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Macro Deck Extension Store API");
            c.RoutePrefix = "docs";
        });
    }
    
}