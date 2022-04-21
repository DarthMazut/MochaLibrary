using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplication.Converters
{
    public class SKBitmapToWirtableBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is SKBitmap skBitmap)
            {
                WriteableBitmap writeableBitmap = new(1, 1);
                writeableBitmap.SetSource(skBitmap.Encode(SKEncodedImageFormat.Png, 0).AsStream().AsRandomAccessStream());
                return writeableBitmap;
            }

            return null;
            //throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is WriteableBitmap writeableBitmap)
            {
                return SKBitmap.Decode(writeableBitmap.PixelBuffer.AsStream());
            }

            return null;
            //throw new ArgumentException();
        }
    }
}
