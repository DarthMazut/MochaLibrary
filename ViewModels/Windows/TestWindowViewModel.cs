using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Windows
{
    public partial class TestWindowViewModel : ObservableObject, ICustomWindowAware<GenericWindowProperties>
    {
        public ICustomWindowControl<GenericWindowProperties> WindowControl { get; } = new CustomWindowControl<GenericWindowProperties>();

        [RelayCommand]
        private void Close()
        {
            WindowControl.Close();
        }

        [RelayCommand]
        private void Maximize()
        {
            WindowControl.Maximize();
        }
    }
}
