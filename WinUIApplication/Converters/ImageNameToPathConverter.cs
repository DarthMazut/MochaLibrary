using Microsoft.UI.Xaml.Data;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinUiApplication.Converters
{
    public class ImageNameToPathConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, string language)
        {
            if (value is null)
            {
                return null;
            }

            if (value is string imageName)
            {
                return Path.Combine(ApplicationSettings.SettingsFolder, imageName);
            }

            throw new ArgumentException();
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
