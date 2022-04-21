using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace WinUiApplication.Converters
{
    public class PathToWritableBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string path)
            {
                if (File.Exists(path))
                {
                    StorageFile file = StorageFile.GetFileFromPathAsync(path).GetAwaiter().GetResult();

                    using (IRandomAccessStream stream = file.OpenAsync(FileAccessMode.ReadWrite).GetAwaiter().GetResult())
                    {
                        WriteableBitmap image = new WriteableBitmap(100, 100);
                        image.SetSource(stream);
                        return image;
                    }
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
