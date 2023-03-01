using MacroDeckExtensionStoreAPI.Extensions;
using MacroDeckExtensionStoreAPI.Middleware;
using MacroDeckExtensionStoreAPI.Startup;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);
await builder.ConfigureAsync();

var app = builder.Build();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());
app.ConfigureSwagger();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

await app.MigrateDatabaseAsync();
await app.RunAsync();
