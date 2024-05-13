using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using MochaCore.Utils.Xaml.UniversalConverter;
using MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplication.Converters.UniversalConverter
{
    public class NumberComparision : MarkupExtension, IConvertingExpression
    {
        private readonly CoreNumberComparision _coreExpression;

        public NumberComparision()
        {
            _coreExpression = new CoreNumberComparision()
            {
                IsEqualTo = IsEqualTo,
                IsNotEqualTo = IsNotEqualTo,
                IsGraterOrEqualTo = IsGraterOrEqualTo,
                IsGreaterThan = IsGreaterThan,
                IsLesserOrEqualTo = IsLesserOrEqualTo,
                IsLesserThan = IsLesserThan
            };
        }

        public int? IsEqualTo { get; set; }

        public int? IsNotEqualTo { get; set; }

        public int? IsGreaterThan { get; set; }

        public int? IsGraterOrEqualTo { get; set; }

        public int? IsLesserThan { get; set; }

        public int? IsLesserOrEqualTo { get; set; }

        public bool IsConditionExpression => _coreExpression.IsConditionExpression;

        public virtual object? CalculateExpression(object? value) => _coreExpression.CalculateExpression(value);

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
