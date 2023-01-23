using MacroDeckExtensionStoreAPI.Extensions;
using MacroDeckExtensionStoreAPI.Startup;


var builder = WebApplication.CreateBuilder(args);
await builder.ConfigureAsync();

var app = builder.Build();

app.ConfigureSwagger();
app.UseAuthorization();
app.MapControllers();
await app.MigrateDatabaseAsync();
await app.RunAsync();