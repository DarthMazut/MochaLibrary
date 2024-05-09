using System;
using System.Collections.Generic;
using System.Linq;

namespace WinUiApplication.Converters.UniversalConverter
{
    public class ConvertingRule
    {
        public object? Condition { get; set; }
        
        public object? Output { get; set; }

        public bool CheckValueMatch(object? value)
        {
            // {x:Type}
            if (Condition is Type type)
            {
                return type == value?.GetType();
            }

            // {c:Expression}
            if (Condition is IConvertingExpression expression)
            {
                if (!expression.IsConditionExpression)
                {
                    throw new Exception($"Not a condition expression.");
                }

                return expression.CalculateExpression(value) is true;
            }

            // $()
            if (Condition is string conditionString && conditionString.StartsWith("$(") && conditionString.EndsWith(")"))
            {
                IConvertingExpression? parsedExpression = UniversalConverter.Expressions
                    .Where(e => e.IsConditionExpression)
                    .Select(e => e.FromExpression(conditionString))
                    .FirstOrDefault(e => e is not null);

                if (parsedExpression is null)
                {
                    throw new Exception("Could not find matching expression.");
                }

                return parsedExpression.CalculateExpression(value) is true;
            }

            // exact value
            return EqualityComparer<object?>.Default.Equals(value, Condition);
        }

        public object? Convert(object? value)
        {
            if (Output is IConvertingExpression expression)
            {
                return expression.CalculateExpression(value);
            }

            if (Condition is string conditionString && conditionString.StartsWith("$(") && conditionString.EndsWith(")"))
            {
                IConvertingExpression? parsedExpression = UniversalConverter.Expressions
                    .Select(e => e.FromExpression(conditionString))
                    .FirstOrDefault(e => e is not null);

                if (parsedExpression is null)
                {
                    throw new Exception("Could not find matching expression.");
                }

                return parsedExpression.CalculateExpression(value);
            }

            return Output;
        }
    }
}
