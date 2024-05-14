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
    public class NumberComparision : MarkupExtension, IConvertingExpression
    {
        public double? IsEqualTo { get; set; }

        public double? IsNotEqualTo { get; set; }

        public double? IsGreaterThan { get; set; }

        public double? IsGraterOrEqualTo { get; set; }

        public double? IsLesserThan { get; set; }

        public double? IsLesserOrEqualTo { get; set; }

        public bool IsConditionExpression => true;

        public virtual object? CalculateExpression(object? value) => new CoreNumberComparision()
        {
            IsEqualTo = IsEqualTo,
            IsNotEqualTo = IsNotEqualTo,
            IsGraterOrEqualTo = IsGraterOrEqualTo,
            IsGreaterThan = IsGreaterThan,
            IsLesserOrEqualTo = IsLesserOrEqualTo,
            IsLesserThan = IsLesserThan
        }.CalculateExpression(value);

        protected override object ProvideValue(IXamlServiceProvider serviceProvider)
        {
            return new NumberComparision()
            {
                IsEqualTo = IsEqualTo,
                IsNotEqualTo = IsNotEqualTo,
                IsGraterOrEqualTo = IsGraterOrEqualTo,
                IsGreaterThan = IsGreaterThan,
                IsLesserOrEqualTo = IsLesserOrEqualTo,
                IsLesserThan = IsLesserThan
            };
        }
    }
}
