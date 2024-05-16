using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using MochaCore.Utils.Xaml.UniversalConverter;
using MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.Utils.Xaml.UniversalConverter
{
    public class Enum : MarkupExtension, IConvertingExpression
    {
        public object? Type { get; set; }

        public object? Value { get; set; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value) => new CoreEnum()
        {
            Type = Type,
            Value = Value
        }.CalculateExpression(value);

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => new Enum()
        {
            Type = Type,
            Value = Value
        };
    }
}
