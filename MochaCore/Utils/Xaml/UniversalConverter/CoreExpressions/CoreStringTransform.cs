using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    public class CoreStringTransform : IConvertingExpression
    {
        public CasingTransform? ToLower { get; init; }

        public CasingTransform? ToUpper { get; init; }

        public string? SplitBy { get; init; }

        public int? SubstringStart { get; init; }

        public int? SubstringEnd { get; init; }

        public TrimmingMode? Trim { get; init; }

        public string? InsertText { get; init; }

        public int? InsertIndex { get; init; }

        // public string? Interpolation { get; init; }
        // {uc:StringTransform Interpolation='Some interpolated string $1 value $2'}

        public bool IsConditionExpression => false;

        public object? CalculateExpression(object? value)
        {
            //Trim
            //Substring
            //Insert
            //ToLower
            //ToUpper
            //SplitBy

            if (value is not string)
            {
                throw new ArgumentException("Expected string as value.");
            }

            string valueString = (value as string)!;

            valueString = Trim switch
            {
                null => valueString,
                TrimmingMode.Whole => valueString.Trim(),
                TrimmingMode.Start => valueString.TrimStart(),
                TrimmingMode.End => valueString.TrimEnd(),
                _ => throw new NotImplementedException()
            };

            int substringStartIndex = SubstringStart ?? 0;
            int substringEndIndex = SubstringEnd ?? valueString.Length;

            valueString = valueString[substringStartIndex..substringEndIndex];

            if (InsertText is string insertText)
            {
                int insertIndex = InsertIndex ?? 0;
                valueString = valueString.Insert(insertIndex, insertText);
            }

            valueString = ToLower switch
            {
                null => valueString,
                CasingTransform.Default => valueString.ToLower(),
                CasingTransform.Invariant => valueString.ToLowerInvariant(),
                _ => throw new NotImplementedException(),
            };

            valueString = ToUpper switch
            {
                null => valueString,
                CasingTransform.Default => valueString.ToUpper(),
                CasingTransform.Invariant => valueString.ToUpperInvariant(),
                _ => throw new NotImplementedException(),
            };

            if (SplitBy is string splitString)
            {
                return valueString.Split(splitString);
            }

            return valueString;
        }
    }

    public enum CasingTransform
    {
        Default,
        Invariant
    }

    public enum TrimmingMode
    {
        Whole,
        Start,
        End
    }
}
