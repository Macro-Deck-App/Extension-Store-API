using MacroDeckExtensionStoreAPI.Startup;


var builder = WebApplication.CreateBuilder(args);
builder.ConfigureAppSettings();
builder.Configure();

var app = builder.Build();

app.ConfigureSwagger();
app.UseAuthorization();
app.MapControllers();
app.Run();