using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplication.Converters.UniversalConverter
{
    // Maybe `NumberComparision`
    public class AritmeticComparisionExpression : MarkupExtension, IConvertingExpression
    {
        // <c:ConvertingRule Condition="{c:AritmeticExpression IsGreaterThan=0 IsLesserThan=10}" />
        // <c:ConvertingRule Condition="$(x > 0 && x < 10)" />

        public int? IsEqualTo { get; set; }

        public int? IsNotEqualTo { get; set; }

        public int? IsGreaterThan { get; set; }

        public int? IsGraterOrEqualTo { get; set; }

        public int? IsLesserThan { get; set; }

        public int? IsLesserOrEqualTo { get; set; }

        public bool IsConditionExpression => true;

        public object? CalculateExpression(object? value)
        {
            decimal? number = null;
            bool isNumber = Microsoft.VisualBasic.Information.IsNumeric(value);
            if (isNumber)
            {
                number = (decimal)value!;
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
                throw new Exception("Cannot resolve value as number");
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

        public IConvertingExpression? FromExpression(string expression)
        {
            // Remove $(...)
            // Split by &&
            // For each ... check if has format [x][sign][singleWord]


            throw new NotImplementedException();
        }

        protected override object ProvideValue(IXamlServiceProvider serviceProvider)
        {
            return new AritmeticComparisionExpression()
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
