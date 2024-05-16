using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using MochaCore.Utils.Xaml.UniversalConverter;
using MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SystemType = System.Type;

namespace MochaWinUI.Utils.Xaml.UniversalConverter
{
    public class Type : MarkupExtension, IConvertingExpression
    {
        public SystemType? TypeValue { get; set; }

        public string? Name { get; set; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value) => new CoreType()
        {
            TypeValue = TypeValue,
            Name = Name
        }.CalculateExpression(value);

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => new Type()
        {
            TypeValue = TypeValue,
            Name = Name
        };
    }
}
