using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    public class CoreType : IConvertingExpression
    {
        public Type? TypeValue { get; init; }

        public string? Name { get; init; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value)
        {
            if (Name is string stringName)
            {
                return Type.GetType(stringName) ?? throw new ArgumentException($"Cannot resolve type of name: {stringName}");
            }

            if (TypeValue is Type type)
            {
                return type;
            }

            throw new ArgumentException("Cannot resolve type");
        }
    }
}
