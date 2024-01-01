
using Microsoft.UI.Xaml.Media.Imaging;
using System.Drawing;
using System.IO;
using Windows.Storage.Streams;

namespace GroceryStore.MainApp.Helpers;

public class BitmapImageHelper
{
    public static async Task<BitmapImage> ToBitmapImageAsync(byte[] data)
    {
        var bitmapImage = new BitmapImage();
        using (var stream = new InMemoryRandomAccessStream())
        {
            using (var writer = new DataWriter(stream))
            {
                writer.WriteBytes(data);
                await writer.StoreAsync();
                await writer.FlushAsync();
                writer.DetachStream();
            }
            stream.Seek(0);
            await bitmapImage.SetSourceAsync(stream);
        }

        return bitmapImage;
    }

    public static async Task<BitmapImage> ToBitmapImageAsync(Stream stream)
    {
        var bitmap = new BitmapImage();
        using (var memStream = new MemoryStream())
        {
            await stream.CopyToAsync(memStream);
            memStream.Position = 0;
            await bitmap.SetSourceAsync(memStream.AsRandomAccessStream());
        }
        return bitmap;
    }
}
