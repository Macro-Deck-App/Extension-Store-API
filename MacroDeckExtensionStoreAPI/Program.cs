using MacroDeckExtensionStoreAPI.Authentication;
using MacroDeckExtensionStoreAPI.Startup;


var builder = WebApplication.CreateBuilder(args);
builder.ConfigureAppSettings();
await builder.ConfigureAsync();

var app = builder.Build();

app.ConfigureSwagger();
app.UseAuthorization();
app.MapControllers();
app.Run();