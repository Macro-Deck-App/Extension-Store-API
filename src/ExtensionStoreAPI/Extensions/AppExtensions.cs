using ExtensionStoreAPI.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExtensionStoreAPI.Extensions;

public static class AppExtensions
{
    public static async Task MigrateDatabaseAsync(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var macroBotContext = scope.ServiceProvider.GetRequiredService<ExtensionStoreDbContext>();
        Log.Information("Starting database migration...");
        await macroBotContext.Database.MigrateAsync();
        Log.Information("Database migration finished");
    }
}