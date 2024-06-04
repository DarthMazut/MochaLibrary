using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    /// <summary>
    /// Allows for numeric comparision of processing value. 
    /// If examined value is collection its length will be evaluated.
    /// If value is enum it will be converted to its numeric equivalent.
    /// If value is string it will be either converted to number or its
    /// length will be evaluated. If no number could be determined from
    /// processing value an exception will be thrown.
    /// </summary>
    public class CoreNumberComparision : IConvertingExpression
    {
        /// <summary>
        /// Checks whether comparing value is equal to provided number.
        /// </summary>
        public double? IsEqualTo { get; init; }

        /// <summary>
        /// Checks whether comparing value is not equal to provided number.
        /// </summary>
        public double? IsNotEqualTo { get; init; }

        /// <summary>
        /// Checks whether comparing value is greater than provided number.
        /// </summary>
        public double? IsGreaterThan { get; init; }

        /// <summary>
        /// Checks whether comparing value is greater or equal to provided number.
        /// </summary>
        public double? IsGraterOrEqualTo { get; init; }

        /// <summary>
        /// Checks whether comparing value is lesser than provided number.
        /// </summary>
        public double? IsLesserThan { get; init; }

        /// <summary>
        /// Checks whether comparing value is lesser or equal to provided number.
        /// </summary>
        public double? IsLesserOrEqualTo { get; init; }

        /// <inheritdoc/>
        public bool IsConditionExpression => true;

        /// <inheritdoc/>
        public virtual object? CalculateExpression(object? value)
        {
            if (!ExpressionUtils.TryRetrieveNumber(value, out double? number))
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
