using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    public class DialogControl : ICustomDialogControl, IDialogControlInitialize
    {
        private bool _isInitialized;
        private IDataContextDialogModule? _module;

        public DialogControl()
        {

        }

        public ICustomDialogModule Module => throw new NotImplementedException();

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
        public event EventHandler Opened;

        /// <inheritdoc/>
        public event EventHandler<CancelEventArgs> Closing;

        public void Close(bool? result)
        {
            throw new NotImplementedException();
        }

        public bool TryClose(bool? result)
        {
            throw new NotImplementedException();
        }

        public IDisposable TrySubscribeDialogClosing(EventHandler<CancelEventArgs> closingHandler)
        {
            throw new NotImplementedException();
        }

        public IDisposable TrySubscribeDialogOpened(EventHandler openedHandler)
        {
            throw new NotImplementedException();
        }

        public IDisposable TrySubscribeDialogOpened(EventHandler openedHandler, Action<IDataContextDialogModule> featureUnavailableHandler)
        {
            throw new NotImplementedException();
        }

        public IDisposable TrySubscriveDialogClosing(EventHandler<CancelEventArgs> closingHandler, Action<IDataContextDialogModule> featureUnavailableHandler)
        {
            throw new NotImplementedException();
        }

        void IDialogControlInitialize.Initialize(IDataContextDialogModule module)
        {
            _module = module;

            InitializeCore();

            _isInitialized = true;
            Initialized?.Invoke(this, EventArgs.Empty);
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

        private void InitializeCore()
        {
            _module!.Opening += ModuleOpening;
            _module!.Closed += ModuleClosed;
            _module!.Disposed += ModuleDisposed;
            //_subscriptionDelegates.ForEach(d => d.SubscribeOrExecute(_module));
        }
    }

    public class DialogControl<T> : DialogControl, ICustomDialogControl<T> where T : new()
    {
        public ICustomDialogModule<T> Module => throw new NotImplementedException();

        public T Properties { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
