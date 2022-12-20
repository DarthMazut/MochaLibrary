using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    /// <summary>
    /// Exposes API for dialog interaction.
    /// </summary>
    /// <typeparam name="TProperties">The type of the dialog properties object associated with the dialog module.</typeparam>
    public class DataContextDialogControl<TProperties> : IDisposable where TProperties : DialogProperties, new()
    {
        private bool _isInitialized;
        private IDataContextDialogModule<TProperties>? _dialogModule;
        private Func<Task>? _onDialogOpenedAsyncDelegate;
        private Action? _onDialogOpenedDelegate;
        private Action? _onDialogClosingDelegate;
        private Func<Task>? _onDialogClosingAsyncDelegate;

        /// <summary>
        /// Determines whether the <see cref="DataContextDialogControl{TProperties}"/> instance has
        /// been initialized and is ready for full interaction.
        /// </summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>
        /// Returns the related <see cref="DataContextDialogControl{TProperties}"/> object.
        /// </summary>
        public IDataContextDialogModule<TProperties> Module
        {
            get
            {
                InitializationGuard();
                return _dialogModule!;
            }
        }

        /// <summary>
        /// Technology-specific dialog object which is represented by related <see cref="IDataContextDialogModule{T}"/>.
        /// </summary>
        public object View
        {
            get
            {
                InitializationGuard();
                return _dialogModule!;
            }
        }

        /// <summary>
        /// Returns statically typed properties as <see cref="DialogProperties"/> object or it's descendant
        /// which allows for configuration of related <see cref="IDataContextDialogModule{T}"/> object.
        /// </summary>
        public TProperties Properties 
        {   
            get
            {
                InitializationGuard();
                return _dialogModule!.Properties;
            } 
        }

        /// <summary>
        /// Should be called only once by <see cref="IDataContextDialogModule{T}"/> related to this instance.
        /// Throws <see cref="InvalidOperationException"/> if this instance has been already initialized.
        /// </summary>
        /// <param name="dialogModule"><see cref="IDataContextDialogModule{T}"/> or it's descendant, which is related to this instance.</param>
        public void Initialize(IDataContextDialogModule<TProperties> dialogModule)
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException($"{nameof(Initialize)} can be called only once.");
            }

            _dialogModule = dialogModule;
            if (_dialogModule is IDialogOpened dialogOpened)
            {
                dialogOpened.Opened += OnDialogOpenedInternal;
            }

            if (_dialogModule is IDialogClosing dialogClosing)
            {
                dialogClosing.Closing += OnDialogClosingInternal;
            }

            _isInitialized = true;
        }

        /// <summary>
        /// Attempts to subscribe to the <see cref="IDialogOpened.Opened"/> event of the related <see cref="IDataContextDialogModule{T}"/>.
        /// Returns <see langword="true"/> if the dialog supports opening event and the subscription was successful,
        /// or <see langword="false"/> if the dialog does not support opening event.
        /// </summary>
        /// <param name="subscribingFunction">The delegate to be executed when the dialog is opened.</param>
        public bool TrySubscribeOnDialogOpened(Action subscribingFunction)
        {
            if (_dialogModule is IDialogOpened)
            {
                _onDialogOpenedDelegate = subscribingFunction;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to subscribe to the <see cref="IDialogOpened.Opened"/> event of the related <see cref="IDataContextDialogModule{T}"/>.
        /// Returns <see langword="true"/> if the dialog supports opening event and the subscription was successful,
        /// or <see langword="false"/> if the dialog does not support opening event.
        /// </summary>
        /// <param name="subscribingFunction">The asynchronous delegate to be executed when the dialog is opened.</param>
        public bool TrySubscribeOnDialogOpened(Func<Task> subscribingFunction)
        {
            if (_dialogModule is IDialogOpened)
            {
                _onDialogOpenedAsyncDelegate = subscribingFunction;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to subscribe to the <see cref="IDialogClosing.Closing"/> event of the related <see cref="IDataContextDialogModule{T}"/>.
        /// Returns <see langword="true"/> if the dialog supports closing event and the subscription was successful,
        /// or <see langword="false"/> if the dialog does not support closing event.
        /// </summary>
        /// <param name="subscribingFunction">The delegate to be executed when the dialog is closing.</param>
        public bool TrySubscribeOnDialogClosing(Action subscribingFunction)
        {
            if (_dialogModule is IDialogClosing)
            {
                _onDialogClosingDelegate = subscribingFunction;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to subscribe to the <see cref="IDialogClosing.Closing"/> event of the related <see cref="IDataContextDialogModule{T}"/>.
        /// Returns <see langword="true"/> if the dialog supports closing event and the subscription was successful,
        /// or <see langword="false"/> if the dialog does not support closing event.
        /// </summary>
        /// <param name="subscribingFunction">The asynchronous delegate to be executed when the dialog is closing.</param>
        public bool TrySubscribeOnDialogClosing(Func<Task> subscribingFunction)
        {
            if (_dialogModule is IDialogClosing)
            {
                _onDialogClosingAsyncDelegate = subscribingFunction;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to close the related <see cref="IDataContextDialogModule{T}"/> if it is currently open. 
        /// Returns <see langword="true"/> if the dialog supports closing (regardless of whether the dialog was
        /// actually closed by calling this method), or <see langword="false"/> if it does not support closing.
        /// Throws an <see cref="InvalidOperationException"/> if DialogControl hasn't been initialized at the time this
        /// method was invoked.
        /// </summary>
        /// <param name="result">Determines the result of dialog interaction.</param>
        public bool TryClose(bool? result)
        {
            InitializationGuard();
            if (_dialogModule is IDialogClose closable)
            {
                closable.Close(); // Need to add Close() with param ;)
                return true;
            }

            return false;
        }

        /// <summary>
        /// Prevents memory leaks by disposing things that need to be disposed.
        /// </summary>
        public void Dispose()
        {
            if (_isInitialized && _dialogModule is IDialogOpened dialogOpened)
            {
                dialogOpened.Opened -= OnDialogOpenedInternal;
            }

            if (_isInitialized && _dialogModule is IDialogOpened dialogClosing)
            {
                dialogClosing.Opened -= OnDialogClosingInternal;
            }

            _dialogModule = null;
            _onDialogOpenedDelegate = null;
            _onDialogOpenedAsyncDelegate = null;
            _onDialogClosingDelegate = null;
            _onDialogClosingAsyncDelegate = null;

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Ensures that the DialogControl has been initialized before allowing a method to be executed.
        /// If the DialogControl is not initialized, an <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        protected void InitializationGuard()
        {
            if (IsInitialized is false) 
            {
                throw new InvalidOperationException("Operation cannot be completed because DialogControl isn't initialized at this time.");
            }
        }

        private async void OnDialogOpenedInternal(object? sender, EventArgs e)
        {
            _onDialogOpenedDelegate?.Invoke();
            if(_onDialogOpenedAsyncDelegate is not null)
            {
                await _onDialogOpenedAsyncDelegate.Invoke();
            }
        }

        private async void OnDialogClosingInternal(object? sender, EventArgs e)
        {
            _onDialogClosingDelegate?.Invoke();
            if (_onDialogClosingAsyncDelegate is not null)
            {
                await _onDialogClosingAsyncDelegate.Invoke();
            }
        }
    }
}
