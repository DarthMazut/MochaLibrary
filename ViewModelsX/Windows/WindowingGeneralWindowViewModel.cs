using CommunityToolkit.Mvvm.ComponentModel;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Windows
{
    public class WindowingGeneralWindowViewModel : ObservableObject, IWindowAware<WindowingGeneralWindowProperties>
    {
        public IWindowControl<WindowingGeneralWindowProperties> WindowControl { get; }
            = new WindowControl<WindowingGeneralWindowProperties>();

        public string Text => "Hello from window :)";
    }
}
