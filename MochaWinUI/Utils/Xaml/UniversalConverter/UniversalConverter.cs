using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Shapes;
using MochaCore.Utils.Xaml.UniversalConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.Utils.Xaml.UniversalConverter
{
    [ContentProperty(Name = nameof(Rules))]
    public class UniversalConverter : IValueConverter
    {
        public List<ConvertingRule> Rules { get; set; } = new();

        public object Convert(object value, System.Type targetType, object parameter, string language)
            => Rules.FirstOrDefault(r => r.CheckValueMatch(value))?.Convert(value) ?? value;

        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
            => throw new NotSupportedException();
    }

    /// <inheritdoc/>
    public class ConvertingRule : CoreRule { }

    /// <inheritdoc/>
    public class ConvertingCondition : CoreCondition { }

    /// <inheritdoc/>
    public class ConvertingOutput : CoreOutput { }
}
