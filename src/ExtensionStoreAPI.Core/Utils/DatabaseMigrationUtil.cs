using EvolveDb;
using ExtensionStoreAPI.Core.Configuration;
using Serilog;

namespace ExtensionStoreAPI.Core.Utils;

public static class DatabaseMigrationUtil
{
    private static readonly ILogger Logger = Log.ForContext(typeof(DatabaseMigrationUtil));
    public static void MigrateDatabase()
    {
        try
        {
            var connectionString = ExtensionStoreApiConfig.DatabaseConnectionStringOverride
                                   ?? ExtensionStoreApiConfig.DatabaseConnectionString;
            var connection = new Npgsql.NpgsqlConnection(connectionString);
            var evolve = new Evolve(connection, msg => Logger.Information(msg))
            {
                Locations = new[] { "Migrations" },
                IsEraseDisabled = true,
                Schemas = new []{ "evolve" }
            };

            evolve.Migrate();
        }
        catch (Exception ex)
        {
            Logger.Fatal(ex, "Database migration failed");
            throw;
        }
    }
}