using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Windows
{
    public partial class WindowingGeneralWindowViewModel : ObservableObject, IWindowAware<WindowingGeneralWindowProperties>
    {
        public IWindowControl<WindowingGeneralWindowProperties> WindowControl { get; }
            = new WindowControl<WindowingGeneralWindowProperties>();

        [ObservableProperty]
        private string _text = "Hello from window :)";
    }
}
