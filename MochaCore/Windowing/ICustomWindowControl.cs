using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Provides API form managing related window.
    /// </summary>
    public interface ICustomWindowControl : IWindowControl, IMaximizeWindow, IMinimizeWindow, IClosingWindow, IHideWindow, IRestoreWindow, IWindowStateAware, IWindowStateChanged
    {
        /// <inheritdoc/>
        IWindowModule IWindowControl.Module => Module;

        /// <summary>
        /// Returns related <see cref="ICustomWindowModule"/> instance.
        /// </summary>
        new public ICustomWindowModule Module { get; }
    }

    /// <summary>
    /// Provides API form managing related window.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public interface ICustomWindowControl<T> : ICustomWindowControl, IWindowControl<T> where T : class, new()
    {
        /// <inheritdoc/>
        ICustomWindowModule ICustomWindowControl.Module => Module;

        /// <inheritdoc/>
        IWindowModule<T> IWindowControl<T>.Module => Module;

        /// <summary>
        /// Returns related <see cref="ICustomWindowModule{T}"/> instance.
        /// </summary>
        new public ICustomWindowModule<T> Module { get; }
    }
}
