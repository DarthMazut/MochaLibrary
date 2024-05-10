using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplication.Converters.UniversalConverter
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

        public virtual object? CalculateExpression(object? value)
        {
            decimal? number = null;
            bool isNumber = Microsoft.VisualBasic.Information.IsNumeric(value);
            if (isNumber)
            {
                number = Convert.ToDecimal(value);
            }

            if (value is string valueString)
            {
                if (decimal.TryParse(valueString, out decimal parsedValue))
                {
                    number = parsedValue;
                }
            }

            if (number is null)
            {
                throw new ArgumentException("Cannot resolve value as number");
            }

            bool result = true;

            if (IsEqualTo is not null)
            {
                result = result && number == IsEqualTo;
            }

            if (IsNotEqualTo is not null)
            {
                result = result && number != IsNotEqualTo;
            }

            if (IsGreaterThan is not null)
            {
                result = result && number > IsGreaterThan;
            }

            if (IsGraterOrEqualTo is not null)
            {
                result = result && number >= IsGraterOrEqualTo;
            }

            if (IsLesserThan is not null)
            {
                result = result && number < IsLesserThan;
            }

            if (IsLesserOrEqualTo is not null)
            {
                result = result && number <= IsLesserOrEqualTo;
            }

            return result;
        }

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
