using ExtensionStoreAPI.Core.DataAccess;
using ExtensionStoreAPI.Core.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ExtensionStoreAPI.Core.Extensions;

public static class DbContextExtensions
{
    private static readonly ILogger Logger = Log.ForContext(typeof(DbContextExtensions));

    public static IQueryable<T> GetNoTrackingSet<T>(this ExtensionStoreDbContext context)
        where T : BaseEntity
    {
        return context.Set<T>().AsNoTracking();
    }

    public static async Task<T> CreateAsync<T>(this ExtensionStoreDbContext context, T obj)
        where T : BaseEntity
    {
        await context.AddAsync(obj);
        await context.SaveAsync();
        return obj;
    }

    public static async Task DeleteAsync<T>(this ExtensionStoreDbContext context, int id)
        where T : BaseEntity
    {
        var existing = await context.Set<T>().FindAsync(id);
        if (existing != null)
        {
            context.Set<T>().Remove(existing);
        }

        await context.SaveAsync();
    }

    public static async Task<T> UpdateAsync<T>(this ExtensionStoreDbContext context, T obj)
        where T : BaseEntity
    {
        try
        {
            context.Entry(obj).State = EntityState.Modified;
            await context.SaveAsync();
        }
        catch (Exception ex)
        {
            Logger.Fatal(ex, "Failed to update entity {Entity}", nameof(T));
        }
        finally
        {
            context.Entry(obj).State = EntityState.Detached;
        }

        return obj;
    }

    private static async Task SaveAsync(this ExtensionStoreDbContext context)
    {
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Logger.Fatal(ex, "Error while saving");
        }
        finally
        {
            context.ChangeTracker.Clear();
        }
    }
}