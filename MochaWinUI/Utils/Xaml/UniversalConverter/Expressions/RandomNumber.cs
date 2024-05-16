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
    public class RandomNumber : MarkupExtension, IConvertingExpression
    {
        public int? LowerBound { get; set; }

        public int? UpperBound { get; set; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value)
            => new CoreRandomNumber()
            {
                LowerBound = LowerBound,
                UpperBound = UpperBound
            }.CalculateExpression(value);

        protected override object ProvideValue(IXamlServiceProvider serviceProvider)
        {
            return new RandomNumber()
            {
                LowerBound = LowerBound,
                UpperBound = UpperBound
            };
        }
    }
}
