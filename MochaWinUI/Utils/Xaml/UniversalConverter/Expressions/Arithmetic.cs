using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using MochaCore.Utils.Xaml.UniversalConverter;
using MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.Utils.Xaml.UniversalConverter.Expressions
{
    public class Arithmetic : MarkupExtension, IConvertingExpression
    {
        public double? Add { get; set; }

        public double? Substract { get; set; }

        public double? Multiply { get; set; }

        public double? Divide { get; set; }

        public double? Modulo { get; set; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value) => new CoreArithmetic()
        {
            Add = Add,
            Substract = Substract,
            Multiply = Multiply,
            Divide = Divide,
            Modulo = Modulo
        }.CalculateExpression(value);

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => new Arithmetic()
        {
            Add = Add,
            Substract = Substract,
            Multiply = Multiply,
            Divide = Divide,
            Modulo = Modulo
        };
    }
}
