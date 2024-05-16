using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    /// <summary>
    /// Returns processing value to <see cref="Of"/> parameter ration as percentage.
    /// Default <see cref="Of"/> value is 1.
    /// </summary>
    public class CorePercentage : IConvertingExpression
    {
        public double? Of { get; init; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value)
        {
            if (ExpressionUtils.TryRetrieveNumber(value, out double? doubleValue))
            {
                return doubleValue * 100 / (Of ?? 1);
            }

            throw new ArgumentException("Cannot resolve value as number."); 
        }
    }
}
