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
    public partial class TestWindowViewModel : ObservableObject, IWindowAware
    {
        public IWindowControl WindowControl { get; } = new WindowControl();

        [RelayCommand]
        private void Close()
        {
            WindowControl.Close();
        }
    }
}
