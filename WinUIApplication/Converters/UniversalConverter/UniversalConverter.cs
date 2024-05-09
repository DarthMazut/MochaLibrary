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
                IConvertingExpression expression1;
                expression1.
            }

            // exact value


        }

        public object Convert(object? value)
        {
            if (Output is string outputString && OutputType is not null && OutputType != typeof(string))
            {
                ITypeInstanceResolver? instanceResolver = UniversalConverter.AvailableTypeInstanceResolvers.FirstOrDefault(r => r.CheckType(OutputType));
                if (instanceResolver is null)
                {
                    throw new Exception($"Cannot create instance of type {OutputType.Name} from string. Are you missing an {nameof(ITypeInstanceResolver)}?");
                }

                return instanceResolver.CreateInstance(OutputType, outputString)!;
            }

            if (Output is not null)
            {
                return Output;
            }

            if (OutputExpression is not null)
            {
                IConvertingExpression? expression 
                    = UniversalConverter.AvailableExpressions.FirstOrDefault(e => e.CheckExpressionMatch(OutputExpression));

                if (expression is null)
                {
                    throw new Exception("Cannot parse expression");
                }

                object? expressionResult = expression.CalculateExpression(OutputExpression, value, InputType)!;

                if (OutputType is not null)
                {
                    ITypeInstanceResolver? instanceResolver = UniversalConverter.AvailableTypeInstanceResolvers.FirstOrDefault(r => r.CheckType(OutputType));
                    return instanceResolver?.CreateInstance(OutputType, expressionResult)
                        ?? throw new Exception($"Cannot create instance of type {OutputType.Name} from string. Are you missing an {nameof(ITypeInstanceResolver)}?");
                }
                else
                {
                    return expressionResult;
                }
            }

            return value!;
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
