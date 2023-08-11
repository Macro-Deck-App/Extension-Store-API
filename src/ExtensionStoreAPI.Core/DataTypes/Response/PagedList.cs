using Microsoft.EntityFrameworkCore;

namespace ExtensionStoreAPI.Core.DataTypes.Response;

public class PagedList<T>
{
    public List<T> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }

    private PagedList(List<T> items, int page, int pageSize, int totalItems)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalItems = totalItems;
    }

    public static async ValueTask<PagedList<T>> CreatePagedListAsync(IQueryable<T> query, int page, int pageSize)
    {
        query = query.Skip((page - 1) * pageSize).Take(pageSize);
        
        var items = await query.ToListAsync();
        var totalItems = await query.CountAsync();

        return new PagedList<T>(items, page, pageSize, totalItems);
    }
}