namespace MochaCore.Utils.Xaml.UniversalConverter
{
    /// <summary>
    /// Provides abstraction over converting expressions utilized by <i>UniversalConverter</i>
    /// implementation.
    /// </summary>
    public interface IConvertingExpression
    {
        /// <summary>
        /// Whether this expression is guaranteed to return <see langword="bool"/>
        /// </summary>
        public bool IsConditionExpression { get; }

        /// <summary>
        /// Evaluate expression to <see langword="object?"/>.
        /// </summary>
        /// <param name="value">Argument of evaluation.</param>
        public object? CalculateExpression(object? value);
    }
}
