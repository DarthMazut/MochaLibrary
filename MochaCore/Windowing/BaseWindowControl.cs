using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Prvoides implementation of <see cref="IBaseWindowControl"/>.
    /// </summary>
    public class BaseWindowControl : IBaseWindowControl, IWindowControlInitialize
    {
        private bool _isInitialized = false;

        protected IBaseWindowModule? _module;
        protected List<SubscriptionDelegate> _subscriptionDelegates = new();
        
        /// <inheritdoc/>
        public event EventHandler? Opened;

        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <inheritdoc/>
        public bool IsInitialized => _isInitialized;

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
        public IBaseWindowModule Module
        {
            get
            {
                InitializationGuard();
                return _module!;
            }
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

        public IDisposable TrySubscribeWindowClosing(EventHandler<CancelEventArgs> closingHandler)
            => TrySubscribeWindowClosing(closingHandler, null);

        public IDisposable TrySubscribeWindowClosing(EventHandler<CancelEventArgs> closingHandler, Action<IBaseWindowModule>? featureUnavailableHandler)
        {
            // What if we're already initialized?

            SubscriptionDelegate subscription = new(m =>
            {
                if (m is IClosingWindow closing)
                {
                    closing.Closing += closingHandler;
                }
                else
                {
                    featureUnavailableHandler?.Invoke(m);
                }
            }, m =>
            {
                if (m is IClosingWindow closing)
                {
                    closing.Closing -= closingHandler;
                }
            });

            _subscriptionDelegates.Add(subscription);
            return subscription;
        }

        public IDisposable TrySubscribeWindowStateChanged(EventHandler<WindowStateChangedEventArgs> stateChangedHandler)
            => TrySubscribeWindowStateChanged(stateChangedHandler, null);

        public IDisposable TrySubscribeWindowStateChanged(EventHandler<WindowStateChangedEventArgs> stateChangedHandler, Action<IBaseWindowModule>? featureUnavailableHandler)
        {
            SubscriptionDelegate subscription = new(m =>
            {
                if (m is IWindowStateChanged stateChanged)
                {
                    stateChanged.StateChanged += stateChangedHandler;
                }
                else
                {
                    featureUnavailableHandler?.Invoke(m);
                }
            }, m =>
            {
                if (m is IWindowStateChanged stateChanged)
                {
                    stateChanged.StateChanged -= stateChangedHandler;
                }
            });

            _subscriptionDelegates.Add(subscription);
            return subscription;
        }

        /// <inheritdoc/>
        void IWindowControlInitialize.Initialize(IBaseWindowModule module)
        {
            _module = module;

            InitializeCore();

            _isInitialized = true;
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
        /// Throws <see cref="InvalidOperationException"/> if current object isn't initialized.
        /// </summary>
        protected void InitializationGuard()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException($"{GetType().Name} was not initialized.");
            }
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
    /// Provides API form managing related window.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public class BaseWindowControl<T> : BaseWindowControl, IBaseWindowControl<T> where T : class, new()
    {
        private Action<T>? _customizeDelegate;

        /// <inheritdoc/>
        public T Properties
        {
            get
            {
                InitializationGuard();
                return ((IBaseWindowModule<T>)_module!).Properties;
            }
        }

        /// <inheritdoc/>
        new public IBaseWindowModule<T> Module => (IBaseWindowModule<T>)base.Module;

        public void Customize(Action<T> customizeDelegate)
        {
            _customizeDelegate = customizeDelegate;
        }

        /// <inheritdoc/>
        protected override void InitializeCore()
        {
            base.InitializeCore();
            _customizeDelegate?.Invoke(Properties);
        }

        // Not sure whether below is neccessary.
        // Initialize() is called by module on its dataContext, and that dataContext is statically typed via module ctor.
        // So it's hard to imagine case where Initialize() gets misstyped module.

        //protected override void InitializeCore()
        //{
        //    if (_module is not IWindowModule<T>)
        //    {
        //        throw new ArgumentException();
        //    }

        //    base.InitializeCore();
        //}
    }
}
