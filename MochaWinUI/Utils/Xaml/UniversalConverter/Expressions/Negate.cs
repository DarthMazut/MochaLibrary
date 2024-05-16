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
    public class Negate : MarkupExtension, IConvertingExpression
    {
        public bool IsConditionExpression => true;

        public object? CalculateExpression(object? value) => new CoreNegate().CalculateExpression(value);

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => new Negate();
    }
}
