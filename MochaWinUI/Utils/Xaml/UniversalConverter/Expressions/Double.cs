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
    public class Double : MarkupExtension, IConvertingExpression
    {
        public double? Value { get; set; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value) => new CoreDouble()
        {
            Value = Value
        }.CalculateExpression(value);

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => new Double()
        {
            Value = Value
        };
    }
}
