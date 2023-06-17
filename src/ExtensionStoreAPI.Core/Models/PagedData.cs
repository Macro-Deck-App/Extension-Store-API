namespace ExtensionStoreAPI.Core.Models;

public class PagedData<T> where T: class
{
    public int TotalItemsCount { get; set; }
    public int MaxPages => (int)Math.Ceiling(TotalItemsCount / (double)ItemsPerPage);
    public int CurrentPage { get; set; }
    public int ItemsPerPage { get; set; }
    public T? Data { get; set; }
}