using System.IO.Compression;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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
    
    public static async Task<Stream?> GeneratePreviewImage(string zipFilePath)
    {
        var zipFile = ZipFile.OpenRead(zipFilePath);
        var icons = zipFile.Entries.Where(f => f.Name.EndsWith(".png") || f.Name.EndsWith(".gif") || f.Name.EndsWith(".jpg")).Take(4);
        const int totalSize = 80;
        using var bitmap = new Image<Rgba32>(totalSize, totalSize, new Rgba32(32, 32, 32));

        const int padding = 2;
        const int iconSize = (80 / 2);

        int row = 0, column = 0;

        var stream = new MemoryStream();
        foreach (var zipIcon in icons)
        {
            try
            {
                using var icon = await Image.LoadAsync(zipIcon.Open());
                using var copy = icon.Clone(x => x.Resize(iconSize - padding, iconSize - padding));
                var x = column * (iconSize + (column * padding));
                var y = row * (iconSize + (row * padding));
                bitmap.Mutate((o) => o.DrawImage(copy, new Point(x, y), 1f));

                column++;
                if (column >= 2)
                {
                    column = 0;
                    row++;
                }
            }
            catch
            {
                continue;
            }
        }
        
        
        await bitmap.SaveAsync(stream, new PngEncoder());
        stream.Position = 0;
        return stream;
    }

}