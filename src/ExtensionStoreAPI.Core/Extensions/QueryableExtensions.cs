using ExtensionStoreAPI.Core.DataAccess.Entities;
using ExtensionStoreAPI.Core.DataTypes.Response;

namespace ExtensionStoreAPI.Core.Extensions;

public static class QueryableExtensions
{
    public static async ValueTask<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int page, int pageSize)
    {
        return await PagedList<T>.CreatePagedListAsync(query, page, pageSize);
    }
    
    public static IQueryable<ExtensionFileEntity> FilterTargetApiVersion(
        this IQueryable<ExtensionFileEntity> query,
        int? targetApiVersion)
    {
        return !targetApiVersion.HasValue 
            ? query
            : query.Where(x => x.MinApiVersion <= targetApiVersion);
    }
}