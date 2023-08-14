using System;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Marks implementing class as data context for <see cref="ICustomWindowModule"/>.
    /// Allows for managing associated window.
    /// </summary>
    public interface ICustomWindowAware : IWindowAware
    {
        /// <inheritdoc/>
        IWindowControl IWindowAware.WindowControl => WindowControl;

        /// <summary>
        /// Provides API form managing related window.
        /// </summary>
        public new ICustomWindowControl WindowControl { get; }
    }

    /// <summary>
    /// Marks implementing class as data context for <see cref="ICustomWindowModule"/>.
    /// Allows for managing associated window.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public interface ICustomWindowAware<T> : ICustomWindowAware, IWindowAware<T> where T : class, new()
    {
        /// <inheritdoc/>
        ICustomWindowControl ICustomWindowAware.WindowControl => WindowControl;

        /// <inheritdoc/>
        IWindowControl<T> IWindowAware<T>.WindowControl => WindowControl;

        /// <summary>
        /// Provides API form managing related window.
        /// </summary>
        public new ICustomWindowControl<T> WindowControl { get; }
    }
}