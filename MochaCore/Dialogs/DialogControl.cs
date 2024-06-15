﻿using MochaCore.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    public class DataContextDialogControl : IDataContextDialogControl, IDialogControlInitialize
    {
        private bool _isInitialized;
        private IDataContextDialogModule? _module;
        private List<LazySubscription> _subscriptionDelegates = new();

        /// <inheritdoc/>
        public object View => Module.View!;

        /// <inheritdoc/>
        public IDataContextDialogModule Module
        {
            get
            {
                InitializationGuard();
                return _module!;
            }
        }

        /// <inheritdoc/>
        public bool IsInitialized => _isInitialized;

        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <inheritdoc/>
        public event EventHandler? Opening;

        /// <inheritdoc/>
        public event EventHandler? Initialized;

        /// <inheritdoc/>
        public bool TryClose(bool? result)
        {
            // TODO: should TryClose throw if module is not initialized?
            // Try* feels like it shouldn't throw, but this is for safe
            // calling method which may not be exposed by module, not
            // sure this should protect from not-initialized exception...

            if (Module is IDialogClose dialogClose)
            {
                dialogClose.Close(result);
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public IDisposable TrySubscribeDialogClosing(EventHandler<CancelEventArgs> closingHandler)
            => TrySubscribeDialogClosing(closingHandler, null);

        /// <inheritdoc/>
        public IDisposable TrySubscribeDialogClosing(EventHandler<CancelEventArgs> closingHandler, Action<IDataContextDialogModule>? featureUnavailableHandler)
            => DeferSubscription(closingHandler, featureUnavailableHandler, nameof(IDialogClosing.Closing));

        /// <inheritdoc/>
        public IDisposable TrySubscribeDialogOpened(EventHandler openedHandler)
            => TrySubscribeDialogOpened(openedHandler, null);

        /// <inheritdoc/>
        public IDisposable TrySubscribeDialogOpened(EventHandler openedHandler, Action<IDataContextDialogModule>? featureUnavailableHandler)
            => DeferSubscription(openedHandler, featureUnavailableHandler, nameof(IDialogOpened.Opened));

        /// <inheritdoc/>
        void IDialogControlInitialize.Initialize(IDataContextDialogModule module) => InitializeCore(module);

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

        /// <summary>
        /// Contains core logic for initialization.
        /// </summary>
        protected void InitializeCore(IDataContextDialogModule module)
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException($"{GetType().Name} has been already initialized");
            }

            _module = module;

            InitializeOverride();

            _isInitialized = true;
            Initialized?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void InitializeOverride()
        {
            _module!.Opening += ModuleOpening;
            _module!.Closed += ModuleClosed;
            _module!.Disposed += ModuleDisposed;
            _subscriptionDelegates.ForEach(d => d.SubscribeOrExecute(_module));
        }

        /// <summary>
        /// Contains core logic of uninitialization.
        /// </summary>
        protected void UninitializeCore()
        {
            UninitializeOverride();

            _module = null;
            _isInitialized = false;
        }

        protected virtual void UninitializeOverride()
        {
            _module!.Opening -= ModuleOpening;
            _module!.Closed -= ModuleClosed;
            _module!.Disposed -= ModuleDisposed;
            _subscriptionDelegates.ForEach(d => d.Dispose());
        }

        private void ModuleOpening(object? sender, EventArgs e) => Opening?.Invoke(this, EventArgs.Empty);

        private void ModuleClosed(object? sender, EventArgs e) => Closed?.Invoke(this, EventArgs.Empty);

        private void ModuleDisposed(object? sender, EventArgs e)
        {     
            if (IsInitialized)
            {
                UninitializeCore();
            }

            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if current object isn't initialized.
        /// </summary>
        protected void InitializationGuard()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException($"{GetType().Name} was not initialized.");
            }
        }
    }

    public class DataContextDialogControl<T> : DataContextDialogControl, IDataContextDialogControl<T>, IDialogControlInitialize where T : new()
    {
        private IDataContextDialogModule<T>? _module;

        /// <inheritdoc/>
        public new IDataContextDialogModule<T> Module
        {
            get
            {
                InitializationGuard();
                return _module!;
            }
        }

        /// <inheritdoc/>
        public T Properties => Module.Properties;

        /// <inheritdoc/>
        void IDialogControlInitialize.Initialize(IDataContextDialogModule module)
        {
            if (module is IDataContextDialogModule<T> typedModule)
            {
                _module = typedModule;
                InitializeCore(module);
            }
            else
            {
                throw new InvalidOperationException($"{typeof(DataContextDialogControl<T>)} cannot be initialized with module which is not {nameof(IDataContextDialogModule<T>)}");
            }         
        }
    }

    public class CustomDialogControl : DataContextDialogControl, ICustomDialogControl, IDialogControlInitialize
    {
        private ICustomDialogModule? _module;

        /// <inheritdoc/>
        public new ICustomDialogModule Module
        {
            get
            {
                InitializationGuard();
                return _module!;
            }
        }

        /// <inheritdoc/>
        public event EventHandler? Opened;

        /// <inheritdoc/>
        public event EventHandler<CancelEventArgs>? Closing;

        /// <inheritdoc/>
        public void Close(bool? result) => Module.Close(result);

        void IDialogControlInitialize.Initialize(IDataContextDialogModule module)
        {
            if (module is ICustomDialogModule typedModule)
            {
                _module = typedModule;
                InitializeCore(module);
            }
            else
            {
                throw new InvalidOperationException($"{typeof(CustomDialogControl)} cannot be initialized with module which is not {nameof(ICustomDialogModule)}");
            }
        }

        /// <inheritdoc/>
        protected override void InitializeOverride()
        {
            base.InitializeOverride();
            _module!.Opened += ModuleOpened;
            _module!.Closing += ModuleClosing;
        }

        /// <inheritdoc/>
        protected override void UninitializeOverride()
        {
            base.UninitializeOverride();
            _module!.Opened -= ModuleOpened;
            _module!.Closing -= ModuleClosing;
        }

        private void ModuleClosing(object? sender, CancelEventArgs e) => Closing?.Invoke(this, e);

        private void ModuleOpened(object? sender, EventArgs e) => Opened?.Invoke(this, e);
    }

    public class CustomDialogControl<T> : CustomDialogControl, ICustomDialogControl<T>, IDialogControlInitialize where T : new()
    {
        private ICustomDialogModule<T>? _module;

        /// <inheritdoc/>
        public new ICustomDialogModule<T> Module
        {
            get
            {
                InitializationGuard();
                return _module!;
            }
        }

        /// <inheritdoc/>
        public T Properties => Module.Properties;

        void IDialogControlInitialize.Initialize(IDataContextDialogModule module)
        {
            if (module is ICustomDialogModule<T> typedModule)
            {
                _module = typedModule;
                InitializeCore(module);
            }
            else
            {
                throw new InvalidOperationException($"{typeof(CustomDialogControl<T>)} cannot be initialized with module which is not {nameof(ICustomDialogModule<T>)}");
            }
        }
    }
}
