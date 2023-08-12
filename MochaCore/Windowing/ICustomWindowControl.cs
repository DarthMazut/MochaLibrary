using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    public interface ICustomWindowControl : IWindowControl, IMaximizeWindow, IMinimizeWindow, IClosingWindow
    {

    }

    public interface ICustomWindowControl<T> : ICustomWindowControl, IWindowControl<T> where T : class, new()
    {

    }
}
