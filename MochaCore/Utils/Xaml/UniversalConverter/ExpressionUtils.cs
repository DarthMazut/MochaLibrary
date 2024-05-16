using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter
{
    /// <summary>
    /// Contains handy methods for <see cref="IConvertingExpression"/> evaluation.
    /// </summary>
    public static class ExpressionUtils
    {
        public static bool TryRetrieveNumber(object? value, out double? number)
        {
            number = null;
            bool isNumber = Microsoft.VisualBasic.Information.IsNumeric(value);
            if (isNumber)
            {
                number = Convert.ToDouble(value);
            }

            if (value is string valueString)
            {
                if (double.TryParse(valueString, out double parsedValue))
                {
                    number = parsedValue;
                }
                else
                {
                    number = valueString.Length;
                }
            }

            if (value is ICollection collection)
            {
                number = collection.Count;
            }
            else if (value is IEnumerable<object?> enumerable)
            {
                number = enumerable.Count();
            }

            if (value?.GetType().IsEnum == true)
            {
                number = (int)value;
            }

            return number is not null;
        }

        //public static bool TryRetrieveType(object? value, out Type? type)
        //{
        //    type = value as Type
        //        ?? Type.GetType(value as string
        //        ?? throw new ArgumentException($"{nameof(Type)} must be either {typeof(SystemType)} or {typeof(string)}."))
        //        ?? throw new ArgumentException($"Provided string does not match any type (?)");
        //}
    }
}
