using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Shapes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplication.Converters.UniversalConverter
{
    [ContentProperty(Name = "Rules")]
    public class UniversalConverter : IValueConverter
    {
        public static List<IConvertingExpression> Expressions { get; set; } = new()
        {

        };

        public List<ConvertingRule> Rules { get; set; } = new();

        public object Convert(object value, Type targetType, object parameter, string language)
            => Rules.FirstOrDefault(r => r.CheckValueMatch(value))?.Convert(value) ?? value;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotSupportedException();
    }

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
                    .FirstOrDefault(e => e.CheckExpressionMatch(conditionString));

                if (parsedExpression is null)
                {
                    throw new Exception("Could not find matching expression.");
                }

                return parsedExpression.CalculateExpression(value) is true;
            }

            // exact value
            return EqualityComparer<object?>.Default.Equals(value, Condition);
        }

        public object Convert(object? value)
        {
            
        }
    }

    public interface IConvertingExpression
    {
        /// <summary>
        /// Whether this expression is guaranteed to return <see langword="bool"/>
        /// </summary>
        public bool IsConditionExpression { get; }

        /// <summary>
        /// Whether this instance can hanlde given expression.
        /// </summary>
        public bool CheckExpressionMatch(string expression);

        /// <summary>
        /// Evaluate expression to <see langword="object?"/>.
        /// </summary>
        public object? CalculateExpression(object? value);

        public IConvertingExpression? FromExpression(string expression);
    }

    public sealed class GreaterThanExpression : IConvertingExpression
    {
        public bool IsConditionExpression => true;

        public bool CheckExpressionMatch(string expression)
        {
            string[] segments = expression.Split();
            if (segments.Length != 3) { return false; }
            if (segments[1] != ">") { return false; }

            return true;
        }

        public object? CalculateExpression(string expression, object? value, Type? inputType)
        {
            string[] segments = expression.Split();
            object? constantParsed = IConvertingExpression.ResolveMostAccurateConstantType(segments[2], value, inputType);

            if (value is IComparable comparable)
            {
                return comparable.CompareTo(constantParsed) > 0;
            }

            throw new Exception("Object is not comparable");
        }
    }

    public interface ITypeInstanceResolver
    {
        public bool CheckType(Type type); 

        public object? CreateInstance(Type type, object definition);
    }

    public class EnumTypeInstanceResolver : ITypeInstanceResolver
    {
        public bool CheckType(Type type) => type.IsEnum;

        public object? CreateInstance(Type type, object definition)
        {
            throw new NotImplementedException();
        }
    }
}
