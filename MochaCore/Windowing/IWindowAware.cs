namespace MochaCore.Windowing
{
    /// <summary>
    /// Marks implementing class as data context for <see cref="IWindowModule"/>.
    /// Allows for managing associated window.
    /// </summary>
    public interface IWindowAware
    {
        /// <summary>
        /// Provides API for managing associated window. 
        /// </summary>
        public IWindowControl WindowControl { get; }
    }

    /// <summary>
    /// Marks implementing class as data context for <see cref="IWindowModule"/>.
    /// Allows for managing associated window.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public interface IWindowAware<T> : IWindowAware where T : class, new()
    {
        /// <summary>
        /// Provides API form managing related window.
        /// </summary>
        public new IWindowControl<T> WindowControl { get; }
    }
}