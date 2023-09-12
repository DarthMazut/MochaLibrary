using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Extends <see cref="IWindowModule"/> with additional capabilities.
    /// </summary>
    public interface ICustomWindowModule : IWindowModule, IMaximizeWindow, IMinimizeWindow, IClosingWindow, IHideWindow, IRestoreWindow, IWindowStateChanged, IWindowStateAware
    {
        /// <inheritdoc/>
        IWindowAware? IWindowModule.DataContext => DataContext;

        /// <summary>
        /// Returns the data context of the representing window or <see langword="null"/> if no such was specified.
        /// </summary>
        new public ICustomWindowAware? DataContext { get; }
    }

    /// <summary>
    /// Extends <see cref="IWindowModule"/> with additional capabilities.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public interface ICustomWindowModule<T> : ICustomWindowModule, IWindowModule<T> where T : class, new()
    {
        /// <inheritdoc/>
        IWindowAware? IWindowModule.DataContext => DataContext;

        /// <inheritdoc/>
        IWindowAware<T>? IWindowModule<T>.DataContext => DataContext;

        /// <inheritdoc/>
        ICustomWindowAware? ICustomWindowModule.DataContext => DataContext;

        /// <summary>
        /// Returns the data context of the representing window or <see langword="null"/> if no such was specified.
        /// </summary>
        new public ICustomWindowAware<T>? DataContext { get; }
    }
}
