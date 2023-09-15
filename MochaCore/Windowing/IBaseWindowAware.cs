namespace MochaCore.Windowing
{
    /// <summary>
    /// Marks implementing class as data context for <see cref="IBaseWindowModule"/>.
    /// Allows for managing associated window.
    /// </summary>
    public interface IBaseWindowAware
    {
        /// <summary>
        /// Provides API for managing associated window. 
        /// </summary>
        public IBaseWindowControl WindowControl { get; }
    }

    /// <summary>
    /// Marks implementing class as data context for <see cref="IBaseWindowModule"/>.
    /// Allows for managing associated window.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public interface IBaseWindowAware<T> : IBaseWindowAware where T : class, new()
    {
        /// <inheritdoc/>
        IBaseWindowControl IBaseWindowAware.WindowControl => WindowControl;

        /// <summary>
        /// Provides API form managing related window.
        /// </summary>
        public new IBaseWindowControl<T> WindowControl { get; }
    }
}