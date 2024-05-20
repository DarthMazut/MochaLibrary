using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    public class CoreDouble : IConvertingExpression
    {
        public double? Value { get; init; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value)
        {
            if (Value is not null)
            {
                return Value;
            }

            if (ExpressionUtils.TryRetrieveNumber(value, out double? doubleValue))
            {
                return doubleValue;
            }

            throw new ArgumentException($"Cannot resolve value as {typeof(double)}");
        }
    }
}
