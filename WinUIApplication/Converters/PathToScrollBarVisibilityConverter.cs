using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplication.Converters
{
    public class PathToScrollBarVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool? isVisible = null;

            if (value is null)
            {
                return ScrollBarVisibility.Disabled;
            }
            else if (value is string path)
            {
                isVisible = File.Exists(path);
            }

            if (isVisible is not null)
            {
                if (parameter is not null)
                {
                    isVisible = !isVisible;
                }

                return isVisible switch
                {
                    true => ScrollBarVisibility.Auto,
                    false => ScrollBarVisibility.Disabled
                };
            }

            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
