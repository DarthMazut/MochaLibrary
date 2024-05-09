namespace WinUiApplication.Converters.UniversalConverter
{
    public interface IConvertingExpression
    {
        /// <summary>
        /// Whether this expression is guaranteed to return <see langword="bool"/>
        /// </summary>
        public bool IsConditionExpression { get; }

        /// <summary>
        /// Returns new instance of <see cref="IConvertingExpression"/> suitable for
        /// provided expression or <see langword="null"/> if expression does not match
        /// current instance.
        /// </summary>
        public IConvertingExpression? FromExpression(string expression);

        /// <summary>
        /// Evaluate expression to <see langword="object?"/>.
        /// </summary>
        public object? CalculateExpression(object? value);  
    }
}
