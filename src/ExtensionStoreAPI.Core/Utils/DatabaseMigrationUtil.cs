using EvolveDb;
using ExtensionStoreAPI.Core.Configuration;
using Serilog;

namespace ExtensionStoreAPI.Core.Utils;

public static class DatabaseMigrationUtil
{
    public static void MigrateDatabase()
    {
        try
        {
            var connectionString = ExtensionStoreApiConfig.DatabaseConnectionStringOverride
                                   ?? ExtensionStoreApiConfig.DatabaseConnectionString;
            var connection = new Npgsql.NpgsqlConnection(connectionString);
            var evolve = new Evolve(connection, Log.Information)
            {
                Locations = new[] { "Migrations" },
                IsEraseDisabled = true,
                Schemas = new []{ "evolve" }
            };

            evolve.Migrate();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Database migration failed");
            throw;
        }
    }
}