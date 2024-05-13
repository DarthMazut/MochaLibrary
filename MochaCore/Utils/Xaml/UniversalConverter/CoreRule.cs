using System;
using System.Collections.Generic;

namespace MochaCore.Utils.Xaml.UniversalConverter;

/// <summary>
/// Represents a single rule defining how the <i>UniversalConverter</i> should handle specific value(s).
/// <para>
/// Each <see cref="CoreRule"/> consists of two main components: the <b>condition</b> and the <b>output</b>.<br/>
/// The condition determines whether the current processed value should be handled by the rule. 
/// The condition can be:
/// <list type="bullet">
/// <item><b>a value</b> - the rule will be applied only if the currently processed value is equal to the value provided as the condition.</item>
/// <item><b>a type</b> - the rule will be applied if the currently processed value is of the type provided as the condition.</item>
/// <item><b>an instance of <see cref="IConvertingExpression"/></b> - the rule will be applied if the provided expression evaluates to <see langword="true"/>.</item>
/// </list>
/// The output specifies the value to be returned if the current rule is selected for handling the processed value.
/// The output can be:
/// <list type="bullet">
/// <item><b>a value</b> - the provided value will be returned as the conversion result.</item>
/// <item><b>an instance of <see cref="IConvertingExpression"/></b> - the value received from evaluating the provided expression will be returned as the conversion result.</item>
/// </list>
/// It is possible to specify multiple conditions and outputs by utilizing the <see cref="Conditions"/> and <see cref="Outputs"/> properties. 
/// When specifying multiple conditions, they are combined using the <b>and</b> operator, meaning all conditions must be met for the rule to process the value.
/// When specifying a collection of output expressions, each expression is evaluated in the order of their declaration, with the argument from the previously evaluated 
/// expression passed to the next expression, effectively creating nested functions, i.e., <c>h(g(f(x)))</c>.
/// <br/><br/>
/// <b>CAUTION: </b> When specifying the <see cref="Condition"/> property, the values in the <see cref="Conditions"/> collection are ignored, 
/// so it's sufficient to specify only one of these properties. Analogously, this applies to <see cref="Output"/> and <see cref="Outputs"/>.
/// </para>
/// </summary>

public class CoreRule
{
    private readonly record struct NoValue;

    /// <summary>
    /// Gets or sets the condition for current rule.
    /// </summary>
    public object? Condition { get; set; } = new NoValue();

    /// <summary>
    /// Gets or sets the output for current rule.
    /// </summary>
    public object? Output { get; set; } = new NoValue();

    /// <summary>
    /// Gets or sets the list of conditions for the current rule.
    /// </summary>
    public List<CoreCondition> Conditions { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of outputs for the current rule.
    /// </summary>
    public List<CoreOutput> Outputs { get; set; } = new();

    /// <summary>
    /// Determines whether the current rule should process the value provided by the <i>UniversalConverter</i>.
    /// </summary>
    /// <param name="value">The value being currently processed by the <i>UniversalConverter</i>.</param>
    /// <returns> <see langword="True"/> if the rule should process the value; otherwise, <see langword="false"/>.</returns>
    public bool CheckValueMatch(object? value)
    {
        if (Condition is not NoValue)
        {
            return CheckSingleCondition(Condition, value);
        }

        bool result = true;
        foreach (CoreCondition condition in Conditions)
        {
            result = result && CheckSingleCondition(condition.Condition, value);
        }

        return result;
    }

    /// <summary>
    /// Determines the output for the current rule.
    /// </summary>
    /// <param name="value">The value to be converted.</param>
    /// <returns>The evaluated output value.</returns>

    public object? Convert(object? value)
    {
        if (Output is not NoValue)
        {
            return ConvertSingleValue(Output, value);
        }

        object? convertingValue = value;
        foreach (CoreOutput outputItem in Outputs)
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

/// <summary>
/// Represents a single condition to be used by the <see cref="CoreRule.Conditions"/> property.
/// </summary>
public class CoreCondition
{
    /// <summary>
    /// Gets or sets the condition encapsulated by this instance.
    /// </summary>
    public object? Condition { get; set; }
}

/// <summary>
/// Represents a single output value or expression to be used by the <see cref="CoreRule.Outputs"/> property.
/// </summary>
public class CoreOutput
{
    /// <summary>
    /// Gets or sets the outptu encapsulated by this instance.
    /// </summary>
    public object? Output { get; set; }
}

