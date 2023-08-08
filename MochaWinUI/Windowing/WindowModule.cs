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

namespace MochaWinUI.Windowing
{
    /// <summary>
    /// Provides WinUI implementation of <see cref="IWindowModule"/>.
    /// </summary>
    public class WindowModule : IWindowModule
    {
        private Window _window;
        private AppWindow? _appWindow;
        private IWindowAware _dataContext;
        private bool _isOpen = false;
        private bool _isDisposed = false;
        private TaskCompletionSource<object?>? _openTaskTsc;
        private object? _result;

        /// <inheritdoc/>
        public event EventHandler? Opened;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public event EventHandler<CancelEventArgs>? Closing;

        public WindowModule(Window window, IWindowAware dataContext)
        {
            _window = window;
            _dataContext = dataContext;
            
            _appWindow = AppWindow.GetFromWindowId(GetWindowId(_window));
            _window.Closed += WindowClosed;
        }

        /// <inheritdoc/>
        public object View => _window;

        /// <inheritdoc/>
        public IWindowAware? DataContext => _dataContext;

        /// <inheritdoc/>
        public bool IsOpen => _isOpen;

        /// <inheritdoc/>
        public bool IsDisposed => _isDisposed;

        /// <inheritdoc/>
        public void Open()
        {
            InitializeDataContext(_dataContext);
            SetDataContext(_dataContext);
            _window.Activate();
            _isOpen = true;

            Opened?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void Open(object parent) => Open();

        /// <inheritdoc/>
        public void Open(IWindowModule parentModule) => Open();

        /// <inheritdoc/>
        public Task OpenAsync()
        {
            _openTaskTsc = new();
            Open();
            return _openTaskTsc.Task;
        }

        /// <inheritdoc/>
        public Task OpenAsync(object parent) => OpenAsync();

        /// <inheritdoc/>
        public Task OpenAsync(IWindowModule parentModule) => OpenAsync();

        /// <inheritdoc/>
        public void Close()
        {
            _window.Close();
        }

        /// <inheritdoc/>
        public void Close(object? result)
        {
            _result = result;
            Close();
        }

        /// <inheritdoc/>
        public void Maximize()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Minimize()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            UninitializeDataContext(_dataContext);
            SetDataContext(null);
            _window.Closed -= WindowClosed;

            _isDisposed = true;
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        private void WindowClosed(object sender, WindowEventArgs args)
        {
            _isOpen = false;
            Closed?.Invoke(this, EventArgs.Empty);
            _openTaskTsc?.SetResult(_result);
        }

        private static WindowId GetWindowId(Window window)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            return Win32Interop.GetWindowIdFromWindow(hWnd);
        }

        private void SetDataContext(IWindowAware? dataContext)
        {
            if (_window.Content is FrameworkElement rootElement)
            {
                rootElement.DataContext = dataContext;
            }
        }

        private void InitializeDataContext(IWindowAware dataContext)
        {
            if (dataContext.WindowControl is WindowControl control)
            {
                control.Initialize(this);
            }
        }

        private void UninitializeDataContext(IWindowAware dataContext)
        {
            if (dataContext.WindowControl is WindowControl control)
            {
                control.Uninitialize();
            }
        }
    }
}
