namespace MacroDeckExtensionStoreLibrary.Models;

public class PagedData<T> where T: class
{
    public int TotalItemsCount { get; set; }
    
    public int MaxPages => TotalItemsCount / ItemsPerPage;
    public int CurrentPage { get; set; }
    public int ItemsPerPage { get; set; }
    public T? Data { get; set; }
}