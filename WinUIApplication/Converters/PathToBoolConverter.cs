using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplication.Converters
{
    public class PathToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool? result = null;

            if (value is null)
            {
                result = false;
            }
            else if (value is string path)
            {
                result = File.Exists(path);
            }

            if (result is not null)
            {
                if (parameter is not null)
                {
                    result = !result;
                }

                return result;
            }

            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
