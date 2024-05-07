using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplication.Converters.UniversalConverter
{
    public class UniversalConverter : IValueConverter
    {
        public List<ConverterRule> ConvertionRules { get; set; } = new();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ConverterRule? matchRule = ConvertionRules.FirstOrDefault(r => r.CheckValueMatch(value));
            if (matchRule is not null)
            {
                return matchRule.Convert(value);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotSupportedException();
    }

    public class ConverterRule
    {
        public object? Input { get; set; }

        public Type? InputType { get; set; }

        public string? InputExpression { get; set; }
        
        public object? Output { get; set; }

        public Type? OutputType { get; set; }

        public string? OutputExpression { get; set; }

        public bool CheckValueMatch(object? value)
        {
            if (Input is not null)
            {
                return Input.Equals(value);
            }

            if (InputExpression is not null)
            {
                ConverterExpression? expression = ConverterExpression.ResolveExpression(InputExpression);
                if (expression is null)
                {
                    throw new Exception("Cannot parse expression");
                }

                return expression.Resolve(value);
            }
            
            return true;
        }

        public object Convert(object? value)
        {
            if (Output is not null)
            {
                return Output;
            }

            if (Output is string && OutputType == typeof(string))
            {
                // TODO:
            }

            if (OutputExpression is not null)
            {
                ConverterExpression? expression = ConverterExpression.ResolveExpression(OutputExpression);
                if (expression is null)
                {
                    throw new Exception("Cannot parse expression");
                }

                return expression.Convert(value);
            }

            return value!;
        }
    }

    //public abstract class ConverterOutputExpression
    //{
    //    private static List<ConverterOutputExpression> _expressions = new()
    //    {
            
    //    };

    //    public static ConverterOutputExpression? ResolveExpression(string expression)
    //        => _expressions.FirstOrDefault(e => e.Match(expression));

    //    public abstract bool Match(string expression);

        

    //}

    public abstract class ConverterExpression
    {
        // Value
        // Operator
        // Constant

        private static List<ConverterExpression> _expressions = new()
        {
            new GreaterThanInputExpression()
        };

        public static ConverterExpression? ResolveExpression(string expression)
            => _expressions.FirstOrDefault(e => e.Match(expression));

        public abstract bool Match(string expression);

        public abstract bool Resolve(object? value);

        public abstract object? Convert(object? value);
    }

    public sealed class GreaterThanInputExpression : ConverterExpression
    {
        public override bool Match(string expression)
        {
            // TODO:
            return true;
        }

        public override bool Resolve(object? value)
        {
            if (value is IComparable comparable)
            {
                return comparable.CompareTo(value) > 0;
            }

            throw new Exception("Object is not comparable");
        }

        public override object? Convert(object? value)
        {
            throw new NotImplementedException();
        }
    }
}
