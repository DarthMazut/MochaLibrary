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
    public class StringTransform : MarkupExtension, IConvertingExpression
    {
        public CasingTransform? ToLower { get; set; }

        public CasingTransform? ToUpper { get; set; }

        public string? SplitBy { get; set; }

        public int? SubstringStart { get; set; }

        public int? SubstringEnd { get; set; }

        public TrimmingMode? Trim { get; set; }

        public string? InsertText { get; set; }

        public int? InsertIndex { get; set; }

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value)
            => new CoreStringTransform()
            {
                ToLower = ToLower,
                ToUpper = ToUpper,
                SplitBy = SplitBy,
                SubstringStart = SubstringStart,
                SubstringEnd = SubstringEnd,
                Trim = Trim,
                InsertText = InsertText,
                InsertIndex = InsertIndex

            }.CalculateExpression(value);

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => new StringTransform()
        {
            ToLower = ToLower,
            ToUpper = ToUpper,
            SplitBy = SplitBy,
            SubstringStart = SubstringStart,
            SubstringEnd = SubstringEnd,
            Trim = Trim,
            InsertText = InsertText,
            InsertIndex = InsertIndex
        };
    }
}
