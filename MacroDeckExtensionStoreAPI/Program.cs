using MacroDeckExtensionStoreAPI.Extensions;
using MacroDeckExtensionStoreAPI.Middleware;
using MacroDeckExtensionStoreAPI.Startup;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);
await builder.ConfigureAsync();

var app = builder.Build();

app.ConfigureSwagger();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ExceptionHandlingMiddleware>();
await app.MigrateDatabaseAsync();
await app.RunAsync();