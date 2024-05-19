using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SystemType = System.Type;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    // TODO: if type is specified but Value is not this should try to parese processing value into enum value.
    public class CoreEnum : IConvertingExpression
    {
        /// <summary>
        /// The type of the enumeration value to be returned.
        /// Can be either <see cref="SystemType"/> or <see cref="string"/> as fully qualified type name.
        /// </summary>
        public object? Type { get; init; }

        /// <summary>
        /// The value of the enumeration to be returned.
        /// Can be either name (<see cref="string"/>) or integer value.
        /// </summary>
        public object? Value { get; init; }

        // TODO: add static type

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value)
        {
            if (Type is null || Value is null)
            {
                throw new ArgumentException($"Both {nameof(Type)} and {nameof(Value)} must be specified.");
            }

            SystemType enumType = 
                Type as SystemType
                ?? SystemType.GetType(Type as string 
                ?? throw new ArgumentException($"{nameof(Type)} must be either {typeof(SystemType)} or {typeof(string)}.")) 
                ?? throw new ArgumentException($"Provided string does not match any type (?)");

            if (!double.TryParse(Value.ToString(), out _) || Value is not string)
            {
                throw new ArgumentException($"{nameof(Value)} must be either number or {typeof(string)}");
            }

            if (Enum.TryParse(enumType, Value.ToString(), out object? result))
            {
                return result;
            }

            throw new ArgumentException($"Couldnt parese {Value} to enum of type {enumType}");
        }
    }
}
