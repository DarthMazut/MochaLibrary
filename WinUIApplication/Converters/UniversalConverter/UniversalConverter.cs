using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Shapes;
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
        public static List<ITypeInstanceResolver> AvailableTypeInstanceResolvers { get; set; } = new()
        {
            new EnumTypeInstanceResolver()
        };

        public static List<IConverterExpressionHandler> AvailableExpressions { get; set; } = new()
        {
            new GreaterThanExpression()
        };

        public List<ConverterRule> Rules { get; set; } = new();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ConverterRule? matchRule = Rules.FirstOrDefault(r => r.CheckValueMatch(value));
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
            if (Input is string inputString && InputType is not null && InputType != typeof(string))
            {
                ITypeInstanceResolver? instanceResolver = UniversalConverter.AvailableTypeInstanceResolvers.FirstOrDefault(r => r.CheckType(InputType));
                if (instanceResolver is null)
                {
                    throw new Exception($"Cannot create instance of type {InputType.Name} from string. Are you missing an {nameof(ITypeInstanceResolver)}?");
                }

                return value?.Equals(instanceResolver.CreateInstance(InputType, inputString)) is true;
            }

            if (Input is not null)
            {
                return Input.Equals(value);
            }

            if (InputExpression is not null)
            {
                IConverterExpressionHandler? expression 
                    = UniversalConverter.AvailableExpressions.Where(e => e.IsBooleanExpression).FirstOrDefault(e => e.CheckExpressionMatch(InputExpression));
                
                if (expression is null)
                {
                    throw new Exception("Cannot parse expression");
                }

                return expression.CalculateExpression(InputExpression, value, InputType) is true;
            }
            
            return true;
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
                IConverterExpressionHandler? expression 
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

    public interface IConverterExpressionHandler
    {
        /// <summary>
        /// Whether this expression is guaranteed to return <see langword="bool"/>
        /// </summary>
        public bool IsBooleanExpression { get; }

        /// <summary>
        /// Whether this instance can hanlde given expression.
        /// </summary>
        public bool CheckExpressionMatch(string expression);

        /// <summary>
        /// Evaluate expression to <see langword="object?"/>.
        /// </summary>
        public object? CalculateExpression(string expression, object? value, Type? inputType);

        public static object? ResolveMostAccurateConstantType(string constantString, object? value, Type? inputType)
        {
            ITypeInstanceResolver? constantTypeResolver;

            if (inputType is not null)
            {
                constantTypeResolver = UniversalConverter.AvailableTypeInstanceResolvers.FirstOrDefault(r => r.CheckType(inputType));
                return constantTypeResolver?.CreateInstance(inputType, constantString)
                    ?? throw new Exception($"Couldn't find {typeof(ITypeInstanceResolver).Name} instance for input type");
            }

            if ( value is not null)
            {
                constantTypeResolver = UniversalConverter.AvailableTypeInstanceResolvers.FirstOrDefault(r => r.CheckType(value.GetType()));
                return constantTypeResolver?.CreateInstance(value.GetType(), constantString);
            }

            if (double.TryParse(constantString, out double doubleValue))
            {
                return doubleValue;
            }

            return constantString;
        }
    }

    public sealed class GreaterThanExpression : IConverterExpressionHandler
    {
        public bool IsBooleanExpression => true;

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
            object? constantParsed = IConverterExpressionHandler.ResolveMostAccurateConstantType(segments[2], value, inputType);

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
