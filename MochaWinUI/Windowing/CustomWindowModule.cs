using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32.Foundation;

namespace MochaWinUI.Windowing
{
    /// <summary>
    /// Provides WinUI implementation of <see cref="IWindowModule"/>.
    /// </summary>
    public class CustomWindowModule : WindowModule, ICustomWindowModule
    {
        public CustomWindowModule(Window window, ICustomWindowAware dataContext) : base(window, dataContext)
        {
            _appWindow.Closing += WindowClosing;
        }

        /// <inheritdoc/>
        public event EventHandler<CancelEventArgs>? Closing;

        /// <inheritdoc/>
        public void Maximize()
        {
            Windows.Win32.PInvoke.ShowWindow(
                (HWND)WinRT.Interop.WindowNative.GetWindowHandle(_window),
                Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD.SW_SHOWMAXIMIZED);
        }

        /// <inheritdoc/>
        public void Minimize()
        {
            Windows.Win32.PInvoke.ShowWindow(
                (HWND)WinRT.Interop.WindowNative.GetWindowHandle(_window),
                Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD.SW_SHOWMINIMIZED);
        }

        /// <inheritdoc/>
        protected override void DisposeCore()
        {
            _appWindow.Closing -= WindowClosing;
        }

        private void WindowClosing(AppWindow sender, AppWindowClosingEventArgs args)
        {
            CancelEventArgs e = new();
            Closing?.Invoke(this, e);
            args.Cancel = e.Cancel;
        }
    }

    public class CustomWindowModule<T> : CustomWindowModule, ICustomWindowModule<T> where T : class, new()
    {
        public CustomWindowModule(Window window, ICustomWindowAware<T> dataContext) : base(window, dataContext)
        {
            Properties = new();
        }

        /// <inheritdoc/>
        public T Properties { get; set; }

        /// <inheritdoc/>
        protected override sealed void OpenCore()
        {
            ApplyProperties();
            OpenCoreOverride();
        }

        /// <inheritdoc/>
        protected virtual void OpenCoreOverride()
        {
            base.OpenCore();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ApplyProperties() { }
    }
}
