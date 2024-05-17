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
    public class StringCheck : MarkupExtension, IConvertingExpression
    {
        public StringCheckOperation? CheckOperation { get; set; }

        public string? StartsWith { get; set; }

        public string? EndsWith { get; set; }

        public string? Contains { get; set; }

        public bool IsConditionExpression => true;

        public object? CalculateExpression(object? value) => new CoreStringCheck()
        {
            CheckOperation = CheckOperation,
            StartsWith = StartsWith,
            EndsWith = EndsWith,
            Contains = Contains
        }.CalculateExpression(value);

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => new StringCheck()
        {
            CheckOperation = CheckOperation,
            StartsWith = StartsWith,
            EndsWith = EndsWith,
            Contains = Contains
        };
    }
}
