using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SystemType = System.Type;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    /// <summary>
    /// Returns the specified value of the enumeration of the provided type.
    /// If a <see cref="Value"/> of the enumeration is not provided, an attempt
    /// to parse the processing value will be made. At least one <c>Type*</c> parameter
    /// is required.
    /// </summary>
    public class CoreEnum : IConvertingExpression
    {
        /// <summary>
        /// The type of the enumeration value to be returned.
        /// </summary>
        public SystemType? Type { get; init; }

        /// <summary>
        /// Fully qualified type name of the enumeration value to be returned.
        /// </summary>
        public string? TypeName { get; init; }

        /// <summary>
        /// The value of the enumeration to be returned.
        /// Can be either name (<see cref="string"/>) or integer value.
        /// </summary>
        public object? Value { get; init; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value)
        {
            if (Type is null && TypeName is null)
            {
                throw new ArgumentException($"Type must be specified. Provide value for either {nameof(Type)} or {nameof(TypeName)} properties.");
            }

            SystemType enumType = Type
                ?? SystemType.GetType(TypeName!) 
                ?? throw new ArgumentException($"Provided string does not match any type (?)");

            string? valueString = Value?.ToString() ?? value?.ToString() ?? null;

            if (Enum.TryParse(enumType, valueString, out object? result))
            {
                return result;
            }

            throw new ArgumentException($"Couldnt parese {valueString} to enum of type {enumType}");
        }
    }
}
