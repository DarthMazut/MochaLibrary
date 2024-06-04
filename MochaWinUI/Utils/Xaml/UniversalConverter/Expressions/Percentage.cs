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
    public class Percentage : MarkupExtension, IConvertingExpression
    {
        public double? Of { get; set; }

        public bool IsConditionExpression => throw new NotImplementedException();

        public object? CalculateExpression(object? value) => new CorePercentage()
        {
            Of = Of
        }.CalculateExpression(value);

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => new Percentage()
        {
            Of = Of
        };
    }
}
