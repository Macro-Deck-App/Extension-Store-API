namespace MacroDeckExtensionStoreLibrary.Utils;

public static class SafeDelete
{
    
    public static void Delete(string path, Action? onFailure = null)
    {
        try
        {
            Retry.Do(() => File.Delete(path), TimeSpan.FromSeconds(2), 3);
        }
        catch
        {
            onFailure?.Invoke();
        }
    }
}