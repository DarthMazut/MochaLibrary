using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    public class CoreNegate : IConvertingExpression
    {
        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }

            if (double.TryParse(value?.ToString(), out double doubleValue))
            {
                return -doubleValue;
            }

            throw new ArgumentException("Only boolean and number values can be negated.");
        }
    }
}
