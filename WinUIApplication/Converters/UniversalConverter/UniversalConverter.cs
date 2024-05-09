using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Shapes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplication.Converters.UniversalConverter
{
    [ContentProperty(Name = nameof(Rules))]
    public class UniversalConverter : IValueConverter
    {
        public static List<IConvertingExpression> Expressions { get; set; } = new()
        {
            new AritmeticComparisionExpression()
        };

        public List<ConvertingRule> Rules { get; set; } = new();

        public object Convert(object value, Type targetType, object parameter, string language)
            => Rules.FirstOrDefault(r => r.CheckValueMatch(value))?.Convert(value) ?? value;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotSupportedException();
    }
}
