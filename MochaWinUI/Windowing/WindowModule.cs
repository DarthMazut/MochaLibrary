using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.Windowing
{
    public class WindowModule : IWindowModule
    {
        protected readonly Window _window;
        protected readonly AppWindow _appWindow;

        private IWindowAware _dataContext;
        private bool _isOpen = false;
        private bool _isDisposed = false;
        private TaskCompletionSource<object?>? _openTaskTsc;
        private object? _result;

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
        public event EventHandler? Opened;

        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <inheritdoc/>
        public void Open()
        {
            DisposedGuard();

            if (!IsOpen)
            {
                InitializeDataContext(_dataContext);
                SetDataContext(_dataContext);

                OpenCore();

                _isOpen = true;
                Opened?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc/>
        public void Open(IWindowModule parentModule) => Open();

        /// <inheritdoc/>
        public void Open(object parent) => Open();

        /// <inheritdoc/>
        public Task OpenAsync()
        {
            DisposedGuard();

            if (!IsOpen)
            {
                _openTaskTsc = new();
                Open();
            }

            return _openTaskTsc?.Task ?? Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task OpenAsync(object parent) => OpenAsync();

        /// <inheritdoc/>
        public Task OpenAsync(IWindowModule parentModule) => OpenAsync();

        /// <inheritdoc/>
        public void Close()
        {
            if (IsOpen)
            {
                CloseCore();
            }
        }

        /// <inheritdoc/>
        public void Close(object? result)
        {
            if (IsOpen)
            {
                _result = result;
                Close();
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                Close();

                DisposeCore();

                UninitializeDataContext(_dataContext);
                SetDataContext(null);
                _window.Closed -= WindowClosed;

                _isDisposed = true;
                Disposed?.Invoke(this, EventArgs.Empty); 
            }
        }

        /// <summary>
        /// Allows to specify additional disposing action. 
        /// </summary>
        protected virtual void DisposeCore() { }

        /// <summary>
        /// Defines how the window is being closed.
        /// </summary>
        protected virtual void CloseCore()
        {
            _window.Close();
        }

        /// <summary>
        /// Defines how the window is being opened.
        /// </summary>
        protected virtual void OpenCore()
        {
            _window.Activate();
        }

        private void WindowClosed(object sender, WindowEventArgs args)
        {
            _isOpen = false;
            Closed?.Invoke(this, EventArgs.Empty);
            _openTaskTsc?.SetResult(_result);
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

        private void DisposedGuard()
        {
            if (IsDisposed)
            {
                throw new InvalidOperationException("Cannot perform operation on disposed object.");
            }
        }

        private static WindowId GetWindowId(Window window)
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            return Win32Interop.GetWindowIdFromWindow(hWnd);
        }
    }

    public class WindowModule<T> : WindowModule, IWindowModule<T> where T : class, new()
    {
        public WindowModule(Window window, IWindowAware dataContext) : base(window, dataContext)
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

        protected virtual void OpenCoreOverride()
        {
            base.OpenCore();
        }

        protected virtual void ApplyProperties() { }
    }
}
