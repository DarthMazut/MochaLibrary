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
    public partial class TestWindowViewModel : ObservableObject, IWindowAware<GenericWindowProperties>
    {
        public IWindowControl<GenericWindowProperties> WindowControl { get; } = new WindowControl<GenericWindowProperties>();

        public TestWindowViewModel()
        {
            WindowControl.Opened += WindowOpened;
            WindowControl.Disposed += WindowDisposed;
            //WindowControl.Closing += WindowClosing;
            WindowControl.TrySubscribeWindowClosing(WindowClosing);
            //WindowControl.StateChanged += (s, e) =>
            //{
            //    Text = $"Window state: {e.WindowState}";
            //};
            WindowControl.TrySubscribeWindowStateChanged((s, e) =>
            {
                Text = $"Window state: {e.WindowState}";
            });
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
            _ = WindowControl.TryMaximize();
        }

        [RelayCommand]
        private async Task Hide()
        {
            //WindowControl.Hide();
            WindowControl.TryHide();
            await Task.Delay(5000);
            //WindowControl.Restore();
            WindowControl.TryRestore();
        }

        private void WindowOpened(object? sender, EventArgs e)
        {
            //Text = $"Window state: {WindowControl.WindowState}";
            if (WindowControl.TryGetState(out ModuleWindowState state))
            {
                Text = $"Window state: {state}";
            }
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
