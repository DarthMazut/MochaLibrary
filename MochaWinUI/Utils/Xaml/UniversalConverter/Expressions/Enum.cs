using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using MochaCore.Utils.Xaml.UniversalConverter;
using MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.Utils.Xaml.UniversalConverter
{
    public class Enum : MarkupExtension, IConvertingExpression
    {
        /// <summary>
        /// The type of the enumeration value to be returned.
        /// </summary>
        public System.Type? Type { get; init; }

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

        public object? CalculateExpression(object? value) => new CoreEnum()
        {
            Type = Type,
            TypeName = TypeName,
            Value = Value
        }.CalculateExpression(value);

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => new Enum()
        {
            Type = Type,
            TypeName = TypeName,
            Value = Value
        };
    }
}
