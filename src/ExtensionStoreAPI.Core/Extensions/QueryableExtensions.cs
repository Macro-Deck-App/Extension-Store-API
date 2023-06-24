using ExtensionStoreAPI.Core.DataTypes.Response;

namespace ExtensionStoreAPI.Core.Extensions;

public static class QueryableExtensions
{
    public static async ValueTask<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int page, int pageSize)
    {
        return await PagedList<T>.CreatePagedListAsync(query, page, pageSize);
    }
}