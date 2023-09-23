using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dispatching;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Windows
{
    public partial class TestWindowViewModel : ObservableObject, IBaseWindowAware<GenericWindowProperties>
    {
        // Skoro i tak trzymamy WindowControl za interfejs, to jak jest róznica, czy ja utworze instancje BaseWindowControl,
        // czy WindowControl? Czy nie wystarczy 1 obiekt WindowControl?
        public IBaseWindowControl<GenericWindowProperties> WindowControl { get; } = new BaseWindowControl<GenericWindowProperties>();

        public TestWindowViewModel()
        {
            WindowControl.Opened += WindowOpened;
            WindowControl.Disposed += WindowDisposed;
            //WindowControl.Closing += WindowClosing;
            //WindowControl.StateChanged += (s, e) =>
            //{
            //    Text = $"Window state: {e.WindowState}";
            //};
        }

        [ObservableProperty]
        private string? _text;

        [RelayCommand]
        private void Close()
        {
            WindowControl.Close("test param");
        }

        [RelayCommand]
        private void Maximize()
        {
            //WindowControl.Maximize();
        }

        [RelayCommand]
        private async Task Hide()
        {
            //WindowControl.Hide();
            await Task.Delay(5000);
            //WindowControl.Restore();
        }

        private void WindowOpened(object? sender, EventArgs e)
        {
            //Text = $"Window state: {WindowControl.WindowState}";
        }

        private void WindowClosing(object? sender, CancelEventArgs e)
        {
            WindowControl.Close("close by framework");
        }

        private void WindowDisposed(object? sender, EventArgs e)
        {
            WindowControl.Opened -= WindowOpened;
           // WindowControl.Closing -= WindowClosing;
            WindowControl.Disposed -= WindowDisposed;
        }
    }
}
