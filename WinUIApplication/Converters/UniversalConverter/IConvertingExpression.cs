namespace WinUiApplication.Converters.UniversalConverter
{
    public interface IConvertingExpression
    {
        /// <summary>
        /// Whether this expression is guaranteed to return <see langword="bool"/>
        /// </summary>
        public bool IsConditionExpression { get; }

        /// <summary>
        /// Evaluate expression to <see langword="object?"/>.
        /// </summary>
        public object? CalculateExpression(object? value);  
    }
}
