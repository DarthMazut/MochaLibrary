using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
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

        public IEnumerable<object?>? Conditions { get; set; } = new List<object?>();

        public IEnumerable<object?>? Outputs { get; set; } = new List<object?>();

        public bool CheckValueMatch(object? value)
        {
            if (Condition is not NoValue)
            {
                return CheckSingleCondition(Condition, value);
            }

            bool result = true;
            foreach (object? condition in Conditions)
            {
                result = result && CheckSingleCondition(condition, value);
            }

            return result;
        }

        public object? Convert(object? value)
        {
            if (Output is not NoValue)
            {
                return ConvertSingleValue(Output, value);
            }

            object? convertingValue = value;
            foreach (object? outputItem in Outputs)
            {
                convertingValue = ConvertSingleValue(outputItem, convertingValue);
            }

            return convertingValue;
        }

        private bool CheckSingleCondition(object? condition, object? value)
        {
            if (condition is NoValue)
            {
                return true;
            }

            if (condition is Type type)
            {
                return type == value?.GetType();
            }

            if (condition is IConvertingExpression expression)
            {
                if (!expression.IsConditionExpression)
                {
                    throw new Exception($"Not a condition expression.");
                }

                return expression.CalculateExpression(value) is true;
            }

            return EqualityComparer<object?>.Default.Equals(value, condition);
        }

        private object? ConvertSingleValue(object? output, object? value)
        {
            if (output is NoValue)
            {
                return value;
            }

            if (output is IConvertingExpression expression)
            {
                return expression.CalculateExpression(value);
            }

            return output;
        }
    }
}
