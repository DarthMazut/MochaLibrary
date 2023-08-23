using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
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
    /// Provides WinUI implementation of <see cref="ICustomWindowModule{T}"/>.
    /// </summary>
    public class CustomWindowModule : ICustomWindowModule
    {
        protected readonly Window _window;
        protected readonly AppWindow _appWindow;

        private IWindowAware? _dataContext;
        private bool _isOpen = false;
        private bool _isDisposed = false;
        private TaskCompletionSource<object?>? _openTaskTsc;
        private object? _result;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWindowControl{T}"/> class.
        /// </summary>
        /// <param name="window">The technology-specific window object associated with the initialized module.</param>
        public CustomWindowModule(Window window) : this(window, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWindowControl{T}"/> class.
        /// </summary>
        /// <param name="window">The technology-specific window object associated with the initialized module.</param>
        /// <param name="dataContext">
        /// The data context for the window associated with the initialized module. If null is passed, 
        /// the data context object will be searched within the provided window object. If the data context of the 
        /// specified window is also null, then the module's data context will be null. 
        /// If the window's data context is of a different type than <see cref="IWindowAware"/>, an exception will be thrown.
        /// </param>
        public CustomWindowModule(Window window, IWindowAware? dataContext)
        {
            _window = window;
            _dataContext = dataContext ?? GetDataContextFromWindow(window);

            _appWindow = AppWindow.GetFromWindowId(GetWindowId(_window));
            _window.Closed += WindowClosed;
            _appWindow.Closing += WindowClosing;
        }

        private IWindowAware? GetDataContextFromWindow(Window window)
        {
            object? windowContext = null;
            if (window.Content is FrameworkElement rootElement)
            {
                windowContext = rootElement.DataContext;
                if (windowContext is not null or IWindowAware)
                {
                    throw new InvalidOperationException($"The data context provided in {window.GetType()} was not of type {typeof(IWindowAware)}");
                }
            }

            return windowContext as IWindowAware;
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
        public event EventHandler<CancelEventArgs>? Closing;

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
        public void Open(IWindowModule parentModule)
        {
            Open();
            Windows.Win32.PInvoke.SetParent(
                (HWND)WinRT.Interop.WindowNative.GetWindowHandle(_window),
                (HWND)WinRT.Interop.WindowNative.GetWindowHandle(parentModule.View));
        }

        /// <inheritdoc/>
        public void Open(object parent) => throw new NotImplementedException();

        /// <inheritdoc/>
        public Task<object?> OpenAsync()
        {
            DisposedGuard();

            if (!IsOpen)
            {
                _openTaskTsc = new();
                Open();
            }

            return _openTaskTsc?.Task ?? Task.FromResult<object?>(null);
        }

        /// <inheritdoc/>
        public Task<object?> OpenAsync(object parent) => throw new NotImplementedException();

        /// <inheritdoc/>
        public Task<object?> OpenAsync(IWindowModule parentModule) => throw new NotImplementedException();

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
                SetDataContext(null);

                _isDisposed = true;
                Disposed?.Invoke(this, EventArgs.Empty); 
            }
        }

        /// <summary>
        /// Defines disposing actions.
        /// </summary>
        protected virtual void DisposeCore()
        {
            _window.Closed -= WindowClosed;
            _appWindow.Closing -= WindowClosing;
        }

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

        private void WindowClosing(AppWindow sender, AppWindowClosingEventArgs args)
        {
            CancelEventArgs e = new();
            Closing?.Invoke(this, e);
            args.Cancel = e.Cancel;
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

        private void InitializeDataContext(IWindowAware? dataContext)
        {
            if (dataContext?.WindowControl is IWindowControlInitialize controlInitialize)
            {
                controlInitialize.Initialize(this);
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

    /// <summary>
    /// Provides WinUI implementation of <see cref="ICustomWindowModule{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of module custom properties.</typeparam>
    public class CustomWindowModule<T> : CustomWindowModule, ICustomWindowModule<T> where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWindowControl{T}"/> class.
        /// </summary>
        /// <param name="window">The technology-specific window object associated with the initialized module.</param>
        public CustomWindowModule(Window window) : this(window, null, new()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWindowControl{T}"/> class.
        /// </summary>
        /// <param name="window">The technology-specific window object associated with the initialized module.</param>
        /// <param name="properties">Provides additional data for module customization.</param>
        public CustomWindowModule(Window window, T properties) : this(window, null, properties) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWindowControl{T}"/> class.
        /// </summary>
        /// <param name="window">The technology-specific window object associated with the initialized module.</param>
        /// <param name="dataContext">
        /// The data context for the window associated with the initialized module. If null is passed, 
        /// the data context object will be searched within the provided window object. If the data context of the 
        /// specified window is also null, then the module's data context will be null. 
        /// If the window's data context is of a different type than <see cref="IWindowAware"/>, an exception will be thrown.
        /// </param>
        public CustomWindowModule(Window window, IWindowAware? dataContext) : this(window, dataContext, new()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWindowControl{T}"/> class.
        /// </summary>
        /// <param name="window">The technology-specific window object associated with the initialized module.</param>
        /// <param name="dataContext">
        /// The data context for the window associated with the initialized module. If null is passed, 
        /// the data context object will be searched within the provided window object. If the data context of the 
        /// specified window is also null, then the module's data context will be null. 
        /// If the window's data context is of a different type than <see cref="IWindowAware"/>, an exception will be thrown.
        /// </param>
        /// <param name="properties">Provides additional data for module customization.</param>
        public CustomWindowModule(Window window, IWindowAware? dataContext, T properties) : base(window, dataContext)
        {
            Properties = properties;
        }

        /// <inheritdoc/>
        public T Properties { get; set; }

        /// <inheritdoc/>
        protected override sealed void OpenCore()
        {
            ApplyProperties();
            OpenCoreOverride();
        }

        /// <summary>
        /// Provides an overridable alternative to the sealed <see cref="OpenCore"/> method.
        /// </summary>
        protected virtual void OpenCoreOverride()
        {
            base.OpenCore();
        }

        /// <summary>
        /// Called just before the associated window is opened.
        /// Override this method to implement logic that applies the <see cref="Properties"/>,
        /// set by the technology-agnostic client code, onto the technology-specific window instance.
        /// </summary>
        protected virtual void ApplyProperties() { }
    }
}
