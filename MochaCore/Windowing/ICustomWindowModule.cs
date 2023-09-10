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

    }

    /// <summary>
    /// Extends <see cref="IWindowModule"/> with additional capabilities.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public interface ICustomWindowModule<T> : ICustomWindowModule, IWindowModule<T> where T : class, new()
    {

    }
}
