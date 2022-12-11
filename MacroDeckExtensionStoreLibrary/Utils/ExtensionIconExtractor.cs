using System.Drawing;
using System.IO.Compression;
using System.Text;

namespace MacroDeckExtensionStoreLibrary.Utils;

public static class ExtensionIconExtractor
{



    public static async Task<Stream?>FromZipFilePathAsync(string zipFilePath)
    {
        var zipFile = ZipFile.OpenRead(zipFilePath);
        var extensionIconFileEntry = zipFile.Entries.FirstOrDefault(x => x.Name.Equals(Constants.ExtensionIconFileName, StringComparison.InvariantCulture));
        if (extensionIconFileEntry == null) return null;
        await using var stream = new StreamReader(extensionIconFileEntry.Open(), Encoding.UTF8).BaseStream;
        Stream outputStream = new MemoryStream();
        await stream.CopyToAsync(outputStream);
        zipFile.Dispose();
        return outputStream;
    }
    
    
}