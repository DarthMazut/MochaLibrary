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
        public int? IsEqualTo { get; set; }

        public int? IsNotEqualTo { get; set; }

        public int? IsGreaterThan { get; set; }

        public int? IsGraterOrEqualTo { get; set; }

        public int? IsLesserThan { get; set; }

        public int? IsLesserOrEqualTo { get; set; }

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
