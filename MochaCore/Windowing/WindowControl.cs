using MochaCore.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Provides implementation for both <see cref="IBaseWindowControl"/> and <see cref="IWindowControl"/> interfaces.
    /// </summary>
    public class WindowControl : IWindowControl, IWindowControlInitialize
    {
        private bool _isInitialized;
        
        protected IBaseWindowModule? _module;
        protected List<LazySubscription> _subscriptionDelegates = new();

        /// <inheritdoc/>
        public event EventHandler? Initialized;

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
        public bool IsInitialized => _isInitialized;

        IWindowModule IWindowControl.Module
        {
            get
            {
                InitializationGuard();
                ModuleTypeGuard<IWindowModule>();
                return (IWindowModule)_module!;
            }
        }

        /// <inheritdoc/>
        public IBaseWindowModule Module
        {
            get
            {
                InitializationGuard();
                return _module!;
            }
        }

        /// <inheritdoc/>
        public object View
        {
            get
            {
                InitializationGuard();
                return _module!.View;
            }
        }

        /// <inheritdoc/>
        public ModuleWindowState WindowState
        {
            get
            {
                InitializationGuard();
                ModuleTypeGuard<IWindowStateAware>();
                return ((IWindowStateAware)_module!).WindowState;
            }
        }

        /// <inheritdoc/>
        public bool TryGetState(out ModuleWindowState windowState)
        {
            windowState = default;

            if (_module is IWindowStateAware stateAware)
            {
                windowState = stateAware.WindowState;
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public void Close()
        {
            InitializationGuard();
            _module!.Close();
        }

        /// <inheritdoc/>
        public void Close(object? result)
        {
            InitializationGuard();
            _module!.Close(result);
        }

        /// <inheritdoc/>
        public void Hide()
        {
            InitializationGuard();
            ModuleTypeGuard<IHideWindow>();
            ((IHideWindow)_module!).Hide();
        }

        /// <inheritdoc/>
        public bool TryHide()
        {
            if (_module is IHideWindow hide)
            {
                hide.Hide();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public void Maximize()
        {
            InitializationGuard();
            ModuleTypeGuard<IMaximizeWindow>();
            ((IMaximizeWindow)_module!).Maximize();
        }

        /// <inheritdoc/>
        public bool TryMaximize()
        {
            if (_module is IMaximizeWindow maximize)
            {
                maximize.Maximize();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public void Minimize()
        {
            InitializationGuard();
            ModuleTypeGuard<IMinimizeWindow>();
            ((IMinimizeWindow)_module!).Minimize();
        }

        /// <inheritdoc/>
        public bool TryMinimize()
        {
            if (_module is IMinimizeWindow minimize)
            {
                minimize.Minimize();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public void Restore()
        {
            InitializationGuard();
            ModuleTypeGuard<IRestoreWindow>();
            ((IRestoreWindow)_module!).Restore();
        }

        /// <inheritdoc/>
        public bool TryRestore()
        {
            if (_module is IRestoreWindow restore)
            {
                restore.Restore();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public IDisposable TrySubscribeWindowClosing(EventHandler<CancelEventArgs> closingHandler)
            => TrySubscribeWindowClosing(closingHandler, null);

        /// <inheritdoc/>
        public IDisposable TrySubscribeWindowClosing(EventHandler<CancelEventArgs> closingHandler, Action<IBaseWindowModule>? featureUnavailableHandler)
            => DeferSubscription(closingHandler, featureUnavailableHandler, nameof(IClosingWindow.Closing));

        /// <inheritdoc/>
        public IDisposable TrySubscribeWindowStateChanged(EventHandler<WindowStateChangedEventArgs> stateChangedHandler)
            => TrySubscribeWindowStateChanged(stateChangedHandler, null);

        /// <inheritdoc/>
        public IDisposable TrySubscribeWindowStateChanged(EventHandler<WindowStateChangedEventArgs> stateChangedHandler, Action<IBaseWindowModule>? featureUnavailableHandler)
            => DeferSubscription(stateChangedHandler, featureUnavailableHandler, nameof(IWindowStateChanged.StateChanged));

        /// <inheritdoc/>
        void IWindowControlInitialize.Initialize(IBaseWindowModule module)
        {
            _module = module;

            InitializeCore();

            _isInitialized = true;
            Initialized?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Contains core logic for initialization.
        /// </summary>
        protected virtual void InitializeCore()
        {
            _module!.Opened += ModuleOpened;
            _module!.Closed += ModuleClosed;
            _module!.Disposed += ModuleDisposed;
            _subscriptionDelegates.ForEach(d => d.SubscribeOrExecute(_module));
        }

        /// <summary>
        /// Contains core logic of uninitialization.
        /// </summary>
        protected virtual void UninitializeCore()
        {
            _module!.Opened -= ModuleOpened;
            _module!.Closed -= ModuleClosed;
            _module!.Disposed -= ModuleDisposed;
            _subscriptionDelegates.ForEach(d => d.Dispose());
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if current object isn't initialized.
        /// </summary>
        protected void InitializationGuard()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException($"{GetType().Name} was not initialized.");
            }
        }


        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if related module is not of provided type.
        /// </summary>
        /// <typeparam name="T">Type of module to be checked against.</typeparam>
        protected void ModuleTypeGuard<T>()
        {
            if (_module is not T)
            {
                throw new InvalidOperationException($"Associated module was expected to be of type {typeof(T)}, but it's not.");
            }
        }

        protected IDisposable DeferSubscription(Delegate eventHandler, Delegate? featureUnavailableHandler, string eventName)
        {
            LazySubscription subscription = new(eventHandler, featureUnavailableHandler, eventName);
            _subscriptionDelegates.Add(subscription);
            if (IsInitialized && _module is not null)
            {
                subscription.SubscribeOrExecute(_module);
            }
            return subscription;
        }

        private void ModuleOpened(object? sender, EventArgs e)
        {
            Opened?.Invoke(this, EventArgs.Empty);
        }

        private void ModuleClosed(object? sender, EventArgs e)
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void ModuleDisposed(object? sender, EventArgs e)
        {
            Disposed?.Invoke(this, EventArgs.Empty);
            if (_isInitialized)
            {
                Uninitialize();
            }
        }

        private void Uninitialize()
        {
            UninitializeCore();

            _module = null;
            _isInitialized = false;
        }
    }

    /// <summary>
    /// Provides implementation for both <see cref="IBaseWindowControl"/> and <see cref="IWindowControl"/> interfaces.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public class WindowControl<T> : WindowControl, IWindowControl<T> where T : class, new()
    {
        private Action<T>? _customizeDelegate;

        /// <inheritdoc/>
        public T Properties
        {
            get
            {
                InitializationGuard();
                ModuleTypeGuard<IBaseWindowModule<T>>();
                return ((IBaseWindowModule<T>)Module).Properties;
            }
        }

        IBaseWindowModule<T> IBaseWindowControl<T>.Module
        {
            get
            {
                InitializationGuard();
                ModuleTypeGuard<IBaseWindowModule<T>>();
                return (IBaseWindowModule<T>)Module;
            }
        }

        IWindowModule<T> IWindowControl<T>.Module
        {
            get
            {
                InitializationGuard();
                ModuleTypeGuard<IWindowModule<T>>();
                return (IWindowModule<T>)Module;
            }
        }

        /// <inheritdoc/>
        public void Customize(Action<T> customizeDelegate) => _customizeDelegate = customizeDelegate;

        /// <inheritdoc/>
        protected override void InitializeCore()
        {
            if (_module is not IBaseWindowModule<T>)
            {
                throw new ArgumentException($"{GetType().Name} can only be initialized with {typeof(IBaseWindowModule<T>)}.");
            }

            base.InitializeCore();
            _customizeDelegate?.Invoke(Properties);
        }
    }
}
