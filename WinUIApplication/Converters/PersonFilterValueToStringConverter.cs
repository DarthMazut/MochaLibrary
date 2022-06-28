using Microsoft.UI.Xaml.Data;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplication.Converters
{
    public class PersonFilterValueToStringConverter : IValueConverter
    {
        private readonly Dictionary<string, PersonFilterValue> _dictionary = new()
        {
            { "Full name", PersonFilterValue.FullName },
            { "First name", PersonFilterValue.FirstName },
            { "Last name", PersonFilterValue.LastName },
            { "Age", PersonFilterValue.Age },
            { "City", PersonFilterValue.City }
        };

        public IList<string> TextValues => _dictionary.Keys.ToList();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is PersonFilterValue enumValue)
            {
                return _dictionary.Where(kvp => kvp.Value == enumValue).First().Key;
            }

            if (value is IList<PersonFilterValue> enumCollection)
            {
                return _dictionary.Keys.ToList();
            }

            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string str)
            {
                return _dictionary[str];
            }

            throw new ArgumentException();
        }
    }
}
