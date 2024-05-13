using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    /// <summary>
    /// Provides a core logic for <i>NumberComparision</i> implementations
    /// per framework.
    /// </summary>
    public class CoreNumberComparision : IConvertingExpression
    {
        /// <summary>
        /// Checks whether comparing value is equal to provided number.
        /// </summary>
        public int? IsEqualTo { get; init; }

        /// <summary>
        /// Checks whether comparing value is not equal to provided number.
        /// </summary>
        public int? IsNotEqualTo { get; init; }

        /// <summary>
        /// Checks whether comparing value is greater than provided number.
        /// </summary>
        public int? IsGreaterThan { get; init; }

        /// <summary>
        /// Checks whether comparing value is greater or equal to provided number.
        /// </summary>
        public int? IsGraterOrEqualTo { get; init; }

        /// <summary>
        /// Checks whether comparing value is lesser than provided number.
        /// </summary>
        public int? IsLesserThan { get; init; }

        /// <summary>
        /// Checks whether comparing value is lesser or equal to provided number.
        /// </summary>
        public int? IsLesserOrEqualTo { get; init; }

        /// <inheritdoc/>
        public bool IsConditionExpression => true;

        /// <inheritdoc/>
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
    }
}
