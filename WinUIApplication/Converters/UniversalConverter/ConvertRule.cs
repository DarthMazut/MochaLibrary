using System;
using System.Collections.Generic;
using System.Linq;

namespace WinUiApplication.Converters.UniversalConverter
{
    public class ConvertRule
    {
        private readonly record struct NoValue;

        public object? Condition { get; set; } = new NoValue();

        public object? Output { get; set; } = new NoValue();

        public bool CheckValueMatch(object? value)
        {
            if (Condition is NoValue)
            {
                return true;
            }

            if (Condition is Type type)
            {
                return type == value?.GetType();
            }

            if (Condition is IConvertingExpression expression)
            {
                if (!expression.IsConditionExpression)
                {
                    throw new Exception($"Not a condition expression.");
                }

                return expression.CalculateExpression(value) is true;
            }

            return EqualityComparer<object?>.Default.Equals(value, Condition);
        }

        public object? Convert(object? value)
        {
            if (Output is NoValue)
            {
                return value;
            }

            if (Output is IConvertingExpression expression)
            {
                return expression.CalculateExpression(value);
            }

            return Output;
        } 
    }
}
