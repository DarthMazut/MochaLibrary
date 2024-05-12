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

        public List<ConvertCondition> Conditions { get; set; } = new();

        public List<ConvertOutput> Outputs { get; set; } = new();

        public bool CheckValueMatch(object? value)
        {
            if (Condition is not NoValue)
            {
                return CheckSingleCondition(Condition, value);
            }

            bool result = true;
            foreach (ConvertCondition condition in Conditions)
            {
                result = result && CheckSingleCondition(condition.Condition, value);
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
            foreach (ConvertOutput outputItem in Outputs)
            {
                convertingValue = ConvertSingleValue(outputItem.Output, convertingValue);
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

    public class ConvertCondition
    {
        public object? Condition { get; set; }
    }

    public class ConvertOutput
    {
        public object? Output { get; set; }
    }
}
