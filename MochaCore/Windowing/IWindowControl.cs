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
    public interface IWindowControl : IBaseWindowControl, IMaximizeWindow, IMinimizeWindow, IClosingWindow, IHideWindow, IRestoreWindow, IWindowStateAware, IWindowStateChanged
    {
        /// <inheritdoc/>
        IBaseWindowModule IBaseWindowControl.Module => Module;

        /// <summary>
        /// Returns related <see cref="IWindowModule"/> instance.
        /// </summary>
        new public IWindowModule Module { get; }
    }

    /// <summary>
    /// Provides API form managing related window.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public interface IWindowControl<T> : IWindowControl, IBaseWindowControl<T> where T : class, new()
    {
        /// <inheritdoc/>
        IWindowModule IWindowControl.Module => Module;

        /// <inheritdoc/>
        IBaseWindowModule<T> IBaseWindowControl<T>.Module => Module;

        /// <summary>
        /// Returns related <see cref="IWindowModule{T}"/> instance.
        /// </summary>
        new public IWindowModule<T> Module { get; }
    }
}
