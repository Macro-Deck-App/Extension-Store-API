using System.Security.Cryptography;

namespace ExtensionStoreAPI.Core.Utils;

public class Sha256Utils
{
    public static async ValueTask<string> CalculateSha256Hash(string path)
    {
        await using var stream = File.OpenRead(path);
        return await CalculateSha256Hash(stream);
    }

    public static async ValueTask<string> CalculateSha256Hash(Stream stream)
    {
        stream.Position = 0;
        
        var bufferedStream = new BufferedStream(stream);
        using var sha256 = SHA256.Create();

        var buffer = new byte[8192];
        int bytesRead;
        while ((bytesRead = await bufferedStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            sha256.TransformBlock(buffer, 0, bytesRead, buffer, 0);
        }
        sha256.TransformFinalBlock(buffer, 0, 0);

        if (sha256.Hash == null)
        {
            throw new InvalidOperationException("Hash was null");
        }
        
        stream.Position = 0;
        return BitConverter.ToString(sha256.Hash).Replace("-", "").ToLower();
    }
}