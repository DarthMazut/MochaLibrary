using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils.Xaml.UniversalConverter.CoreExpressions
{
    public class CoreStringCheck : IConvertingExpression
    {
        public StringCheckOperation? CheckOperation { get; init; }

        public string? StartsWith { get; init; }

        public string? EndsWith { get; init; }

        public string? Contains { get; init; }

        public bool IsConditionExpression => true;

        public object? CalculateExpression(object? value)
        {
            if (value is string stringValue)
            {
                bool? result = null;
                if (StartsWith is string startsWith)
                {
                    result = (result ?? true) && stringValue.StartsWith(startsWith);
                }

                if (EndsWith is string endsWith)
                {
                    result = (result ?? true) && stringValue.EndsWith(endsWith);
                }

                if (Contains is string contains)
                {
                    result = (result ?? true) && stringValue.Contains(contains);
                }

                bool? switchResult = CheckOperation switch
                {
                    null => null,
                    StringCheckOperation.IsNullOrEmpty => string.IsNullOrEmpty(stringValue),
                    StringCheckOperation.IsNullOrWhitespace => string.IsNullOrWhiteSpace(stringValue),
                    StringCheckOperation.IsLowerCase => stringValue.All(c => char.IsLower(c)),
                    StringCheckOperation.IsUpperCase => stringValue.All(c => char.IsUpper(c)),
                    _ => throw new NotImplementedException(),
                };

                return (result ?? true) && (switchResult ?? true);
            }

            throw new ArgumentException($"Value must be {typeof(string)}");
        }
    }

    public enum StringCheckOperation
    {
        IsNullOrEmpty,
        IsNullOrWhitespace,
        IsLowerCase,
        IsUpperCase
    }
}
