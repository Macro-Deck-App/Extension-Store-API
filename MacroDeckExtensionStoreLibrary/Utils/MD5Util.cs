using System.Security.Cryptography;

namespace MacroDeckExtensionStoreLibrary.Utils;

public static class MD5Util
{

    public static async Task<string> GetMD5HashAsync(string path)
    {
        await using var stream = File.OpenRead(path);
        using var md5 = MD5.Create();
        var hash = await md5.ComputeHashAsync(stream);
        var checksumString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        return checksumString;
    }
    
    
}