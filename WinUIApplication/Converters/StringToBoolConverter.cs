using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplication.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool returnValue = false;

            if (value is not null)
            {
                if (value is string str)
                {
                    returnValue = !string.IsNullOrWhiteSpace(str);
                }
                else
                {
                    throw new ArgumentException();
                }
            }

            if (parameter is not null)
            {
                returnValue = !returnValue;
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
