using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Extends <see cref="IBaseWindowModule"/> with additional capabilities.
    /// </summary>
    public interface IWindowModule : IBaseWindowModule, IMaximizeWindow, IMinimizeWindow, IClosingWindow, IHideWindow, IRestoreWindow, IWindowStateChanged, IWindowStateAware
    {
        /// <inheritdoc/>
        IBaseWindowAware? IBaseWindowModule.DataContext => DataContext;

        /// <summary>
        /// Returns the data context of the representing window or <see langword="null"/> if no such was specified.
        /// </summary>
        new public IWindowAware? DataContext { get; }
    }

    /// <summary>
    /// Extends <see cref="IBaseWindowModule"/> with additional capabilities.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public interface IWindowModule<T> : IWindowModule, IBaseWindowModule<T> where T : class, new()
    {
        /// <inheritdoc/>
        IBaseWindowAware? IBaseWindowModule.DataContext => DataContext;

        /// <inheritdoc/>
        IBaseWindowAware<T>? IBaseWindowModule<T>.DataContext => DataContext;

        /// <inheritdoc/>
        IWindowAware? IWindowModule.DataContext => DataContext;

        /// <summary>
        /// Returns the data context of the representing window or <see langword="null"/> if no such was specified.
        /// </summary>
        new public IWindowAware<T>? DataContext { get; }
    }
}
