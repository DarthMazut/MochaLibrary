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
    /// Provides WinUI implementation of <see cref="IWindowModule{T}"/>.
    /// </summary>
    public class WindowModule : IWindowModule
    {
        protected readonly Window _window;

        private IWindowAware? _dataContext;
        private bool _isOpen = false;
        private bool _isDisposed = false;
        private ModuleWindowState _state;
        private TaskCompletionSource<object?>? _openTaskTsc;
        private object? _result;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowControl{T}"/> class.
        /// </summary>
        /// <param name="window">The technology-specific window object associated with the initialized module.</param>
        public WindowModule(Window window) : this(window, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowControl{T}"/> class.
        /// </summary>
        /// <param name="window">The technology-specific window object associated with the initialized module.</param>
        /// <param name="dataContext">
        /// The data context for the window associated with the initialized module. If null is passed, 
        /// the data context object will be searched within the provided window object. If the data context of the 
        /// specified window is also null, then the module's data context will be null. 
        /// If the window's data context is of a different type than <see cref="IBaseWindowAware"/>, an exception will be thrown.
        /// </param>
        public WindowModule(Window window, IWindowAware? dataContext)
        {
            _window = window;
            _dataContext = dataContext ?? GetDataContextFromWindow(window);

            _window.Closed += WindowClosed;
            _window.AppWindow.Closing += WindowClosing;
            _window.AppWindow.Changed += WindowChanged;
        }

        /// <inheritdoc/>
        public object View => _window;

        /// <inheritdoc/>
        public IWindowAware? DataContext => _dataContext;

        /// <inheritdoc/>
        public bool IsOpen => _isOpen;

        /// <inheritdoc/>
        public ModuleWindowState WindowState => _state;

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
        public event EventHandler<WindowStateChangedEventArgs>? StateChanged;

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
                UpdateState();
                Opened?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc/>
        public void Open(IBaseWindowModule parentModule) => throw new NotImplementedException();

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
        public Task<object?> OpenAsync(IBaseWindowModule parentModule) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void Maximize()
        {
            (_window.AppWindow.Presenter as OverlappedPresenter)?.Maximize();
        }

        /// <inheritdoc/>
        public void Minimize()
        {
            (_window.AppWindow.Presenter as OverlappedPresenter)?.Minimize();
        }

        /// <inheritdoc/>
        public void Hide()
        {
            _window.AppWindow.Hide();
        }

        /// <inheritdoc/>
        public void Restore()
        {
            (_window.AppWindow.Presenter as OverlappedPresenter)?.Restore(true);
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
            _window.AppWindow.Closing -= WindowClosing;
            _window.AppWindow.Changed -= WindowChanged;
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

        /// <summary>
        /// Defines how to determine current state of related <see cref="Window"/>.
        /// </summary>
        protected virtual ModuleWindowState ResolveCurrentState()
        {
            if (IsOpen)
            {
                if (!_window.AppWindow.IsVisible)
                {
                    return ModuleWindowState.Hidden;
                }

                if (_window.AppWindow.Presenter is OverlappedPresenter overlappedPresenter)
                {
                    if (overlappedPresenter.IsModal)
                    {
                        return ModuleWindowState.Modal;
                    }

                    return overlappedPresenter.State switch
                    {
                        OverlappedPresenterState.Maximized => ModuleWindowState.Maximized,
                        OverlappedPresenterState.Minimized => ModuleWindowState.Minimized,
                        OverlappedPresenterState.Restored => ModuleWindowState.Normal,
                        _ => ModuleWindowState.Normal,
                    };
                }

                if (_window.AppWindow.Presenter is FullScreenPresenter)
                {
                    return ModuleWindowState.FullScreen;
                }

                if (_window.AppWindow.Presenter is CompactOverlayPresenter)
                {
                    return ModuleWindowState.Floating;
                }

                return ModuleWindowState.Normal;
            }

            return ModuleWindowState.Closed;
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
            UpdateState();
            Closed?.Invoke(this, EventArgs.Empty);
            _openTaskTsc?.SetResult(_result);
        }

        private void WindowChanged(AppWindow sender, AppWindowChangedEventArgs e)
        {
            UpdateState();
        }

        private void UpdateState()
        {
            ModuleWindowState currentState = ResolveCurrentState();
            if (_state != currentState)
            {
                _state = currentState;
                StateChanged?.Invoke(this, new WindowStateChangedEventArgs(currentState));
            }
        }

        private void SetDataContext(IBaseWindowAware? dataContext)
        {
            if (_window.Content is FrameworkElement rootElement)
            {
                rootElement.DataContext = dataContext;
            }
        }

        private IWindowAware? GetDataContextFromWindow(Window window)
        {
            object? windowContext = null;
            if (window.Content is FrameworkElement rootElement)
            {
                windowContext = rootElement.DataContext;
                if (windowContext is not null or IBaseWindowAware)
                {
                    throw new InvalidOperationException($"The data context provided in {window.GetType()} was not of type {typeof(IBaseWindowAware)}");
                }
            }

            return windowContext as IWindowAware;
        }

        private void InitializeDataContext(IBaseWindowAware? dataContext)
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
    /// Provides WinUI implementation of <see cref="IWindowModule{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of module custom properties.</typeparam>
    public class WindowModule<T> : WindowModule, IWindowModule<T> where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowControl{T}"/> class.
        /// </summary>
        /// <param name="window">The technology-specific window object associated with the initialized module.</param>
        public WindowModule(Window window) : this(window, null, new()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowControl{T}"/> class.
        /// </summary>
        /// <param name="window">The technology-specific window object associated with the initialized module.</param>
        /// <param name="properties">Provides additional data for module customization.</param>
        public WindowModule(Window window, T properties) : this(window, null, properties) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowControl{T}"/> class.
        /// </summary>
        /// <param name="window">The technology-specific window object associated with the initialized module.</param>
        /// <param name="dataContext">
        /// The data context for the window associated with the initialized module. If null is passed, 
        /// the data context object will be searched within the provided window object. If the data context of the 
        /// specified window is also null, then the module's data context will be null. 
        /// If the window's data context is of a different type than <see cref="IWindowAware{T}"/>, an exception will be thrown.
        /// </param>
        public WindowModule(Window window, IWindowAware<T>? dataContext) : this(window, dataContext, new()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowControl{T}"/> class.
        /// </summary>
        /// <param name="window">The technology-specific window object associated with the initialized module.</param>
        /// <param name="dataContext">
        /// The data context for the window associated with the initialized module. If null is passed, 
        /// the data context object will be searched within the provided window object. If the data context of the 
        /// specified window is also null, then the module's data context will be null. 
        /// If the window's data context is of a different type than <see cref="IWindowAware{T}"/>, an exception will be thrown.
        /// </param>
        /// <param name="properties">Provides additional data for module customization.</param>
        public WindowModule(Window window, IWindowAware<T>? dataContext, T properties) : base(window, dataContext)
        {
            Properties = properties;
        }

        /// <summary>
        /// Customizes view object based on properties within <see cref="Properties"/>.
        /// Use this delagate to avoiding subcalssing only for overriding <see cref="ApplyPropertiesCore"/>.
        /// </summary>
        public Action<Window, T>? ApplyProperties { get; init; }

        /// <inheritdoc/>
        public T Properties { get; set; }

        /// <inheritdoc/>
        new public IWindowAware<T>? DataContext => base.DataContext is not null ? (IWindowAware<T>)base.DataContext : null;

        /// <inheritdoc/>
        protected override sealed void OpenCore()
        {
            ApplyPropertiesCore();
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
        protected virtual void ApplyPropertiesCore()
        {
            ApplyProperties?.Invoke(_window, Properties);
        }
    }
}
