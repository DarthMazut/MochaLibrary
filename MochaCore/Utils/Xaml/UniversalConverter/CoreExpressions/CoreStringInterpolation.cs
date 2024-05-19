using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    public class CoreStringInterpolation : IConvertingExpression
    {
        public string? String { get; init; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value)
        {
            if (String is null)
            {
                throw new ArgumentException($"Property {nameof(String)} cannot be null");
            }

            if (value is IEnumerable<object?> enumerable)
            {
                return string.Format(String, enumerable.Select(i => i.ToString()).ToArray());
            }
            else
            {
                return string.Format(String, value?.ToString());
            }
        }
    }
}
