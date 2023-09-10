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
    public partial class TestWindowViewModel : ObservableObject, ICustomWindowAware<GenericWindowProperties>
    {
        public ICustomWindowControl<GenericWindowProperties> WindowControl { get; } = new CustomWindowControl<GenericWindowProperties>();

        public TestWindowViewModel()
        {
            WindowControl.Opened += WindowOpened;
            WindowControl.Closing += WindowClosing;
            WindowControl.Disposed += WindowDisposed;
            WindowControl.StateChanged += (s, e) =>
            {
                Text = $"Window state: {e.WindowState}";
            };
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
            WindowControl.Maximize();
        }

        [RelayCommand]
        private async Task Hide()
        {
            WindowControl.Hide();
            await Task.Delay(5000);
            WindowControl.Restore();
        }

        private async void WindowOpened(object? sender, EventArgs e)
        {
            Text = $"Window state: {WindowControl.WindowState}";
            //await Task.Delay(3000).ContinueWith(t =>
            //{
            //    DispatcherManager.GetMainThreadDispatcher().EnqueueOnMainThread(() =>
            //    {
            //        Text = "Test!";
            //        WindowControl.Properties.Info = "xyz...";
            //    });
            //});
        }

        private void WindowClosing(object? sender, CancelEventArgs e)
        {
            WindowControl.Close("close by framework");
        }

        private void WindowDisposed(object? sender, EventArgs e)
        {
            WindowControl.Opened -= WindowOpened;
            WindowControl.Closing -= WindowClosing;
            WindowControl.Disposed -= WindowDisposed;
        }
    }
}
