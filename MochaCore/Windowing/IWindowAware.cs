using System;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Marks implementing class as data context for <see cref="IWindowModule"/>.
    /// Allows for managing associated window.
    /// </summary>
    public interface IWindowAware : IBaseWindowAware
    {
        /// <inheritdoc/>
        IBaseWindowControl IBaseWindowAware.WindowControl => WindowControl;

        /// <summary>
        /// Provides API form managing related window.
        /// </summary>
        public new IWindowControl WindowControl { get; }
    }

    /// <summary>
    /// Marks implementing class as data context for <see cref="IWindowModule"/>.
    /// Allows for managing associated window.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public interface IWindowAware<T> : IWindowAware, IBaseWindowAware<T> where T : class, new()
    {
        /// <inheritdoc/>
        IBaseWindowControl IBaseWindowAware.WindowControl => WindowControl;

        /// <inheritdoc/>
        IWindowControl IWindowAware.WindowControl => WindowControl;

        /// <inheritdoc/>
        IBaseWindowControl<T> IBaseWindowAware<T>.WindowControl => WindowControl;

        /// <summary>
        /// Provides API form managing related window.
        /// </summary>
        public new IWindowControl<T> WindowControl { get; }
    }
}