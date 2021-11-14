using Microsoft.UI.Xaml;
using MochaCore.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MochaCoreWinUI.Dialogs
{
    /// <summary>
    /// Provides base class for <see cref="OpenFileDialogModuleOld"/> and <see cref="SaveFileDialogModule"/>.
    /// </summary>
    /// <typeparam name="TView">Type of underlying technology-specifis dialog object.</typeparam>
    /// <typeparam name="TControl">Type of <see cref="DialogControl"/> object.</typeparam>
    public abstract class FileDialogModuleBase<TView, TControl> : IDialogModule<TControl> where TControl : DialogControl
    {
        private Window _parentWindow;
        private TView _view;
        private IDialog<TControl> _dataContext;
        private bool _isOpen = false;
        private bool _isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDialogModuleBase{TView, TControl}"/> class.
        /// </summary>
        /// <param name="parentWindow">Parent window.</param>
        /// <param name="view">Technology-specific dialog object.</param>
        public FileDialogModuleBase(Window parentWindow, TView view)
        {
            _parentWindow = parentWindow;
            _view = view;
            _dataContext = new SimpleDialogData<TControl>();
        }

        /// <inheritdoc/>
        public object? View => _view;

        /// <inheritdoc/>
        public IDialog<TControl> DataContext => _dataContext;

        /// <inheritdoc/>
        IDialog IDialogModule.DataContext => _dataContext;

        /// <inheritdoc/>
        public bool IsOpen => _isOpen;

        /// <inheritdoc/>
        public event EventHandler? Opening;

        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <summary>
        /// This method is not supported for <see cref="FileDialogModuleBase{TView, TControl}"/> implementation.
        /// </summary>
        public event EventHandler? Opened
        {
            add => throw new InvalidOperationException($"{nameof(Opened)} event is not supported by {GetType().Name}.");
            remove => throw new InvalidOperationException($"{nameof(Opened)} event is not supported by {GetType().Name}.");
        }

        /// <summary>
        /// This method is not supported for <see cref="FileDialogModuleBase{TView, TControl}"/> implementation.
        /// </summary>
        public event EventHandler<CancelEventArgs>? Closing
        {
            add => throw new InvalidOperationException($"{nameof(Closing)} event is not supported by {GetType().Name}.");
            remove => throw new InvalidOperationException($"{nameof(Closing)} event is not supported by {GetType().Name}.");
        }

        /// <summary>
        /// A delegate which customizes a view object based on values from
        /// provided <see cref="DialogControl"/> object.
        /// </summary>
        public Action<TView, TControl>? CustomizeDelegate { get; set; }

        /// <summary>
        /// A delegate which handles the result from view object. Proper values should be assigned to
        /// <see cref="DialogControl"/> object as well.
        /// </summary>
        public Func<StorageFile, TView, TControl, bool?>? HandleResultDelgate { get; set; }

        /// <inheritdoc/>
        public void Close()
        {
            throw new InvalidOperationException($"{GetType().Name} does not support Close() method.");
        }

        /// <summary>
        /// Perform cleaning operations allowing this object to be garbage collected.
        /// </summary>
        public void Dispose()
        {
            _dataContext.DialogControl.Dispose();
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void SetDataContext(IDialog dialog)
        {
            if (dialog is IDialog<TControl> typedDialog)
            {
                _dataContext = typedDialog;
                _dataContext.DialogControl.Activate(this);
            }
            else
            {
                throw new ArgumentException($"Data context for {GetType().Name} can only be of type {typeof(IDialog<TControl>)}");
            }
        }

        /// <summary>
        /// This method is not supported for <see cref="FileDialogModuleBase{TView, TControl}"/> implementation.
        /// </summary>
        public void Show()
        {
            throw new InvalidOperationException($"{GetType().Name} does not support synchronous Show() method.");
        }

        /// <summary>
        /// This method is not supported for <see cref="FileDialogModuleBase{TView, TControl}"/> implementation.
        /// </summary>
        public Task ShowAsync()
        {
            throw new InvalidOperationException($"{GetType().Name} does not support ShowAsync() method.");
        }

        /// <summary>
        /// This method is not supported for <see cref="FileDialogModuleBase{TView, TControl}"/> implementation.
        /// </summary>
        public bool? ShowModal()
        {
            throw new InvalidOperationException($"{GetType().Name} does not support synchronous ShowModal() method.");
        }

        /// <inheritdoc/>
        public async Task<bool?> ShowModalAsync()
        {
            if (_isOpen)
            {
                throw new InvalidOperationException("Cannot show already opened dialog");
            }

            if (_isDisposed)
            {
                throw new InvalidOperationException("Cannot show already disposed dialog");
            }

            // Workaround for bug https://github.com/microsoft/WindowsAppSDK/issues/466
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(_parentWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(_view, hwnd);

            Customize();
            Opening?.Invoke(this, EventArgs.Empty);
            _isOpen = true;
            StorageFile result = await ShowDialogCore(_view);
            _isOpen = false;
            Closed?.Invoke(this, EventArgs.Empty);
            return HandleResult(result);
        }

        /// <summary>
        /// Customizes view object based on properties within <see cref="DialogControl"/>.
        /// </summary>
        /// <param name="view">View object to be customized.</param>
        /// <param name="dialogControl">A <see cref="DialogControl"/> object which serves as a source for customization.</param>
        protected abstract void CustomizeCore(TView view, TControl dialogControl);

        /// <summary>
        /// Translates technology-specific dialog result object into technology-independent <see langword="bool?"/> value.
        /// Sets the dialog results within <see cref="DialogControl"/> object.
        /// </summary>
        /// <param name="result">Result to be translated.</param>
        /// <param name="dialogControl">Results should be set on this object.</param>
        protected abstract bool? HandleResultCore(StorageFile result, TView view, TControl dialogControl);

        /// <summary>
        /// When overriden should show technology-specific dialog object and return it's result.
        /// </summary>
        /// <param name="view">Technology-specific dialog object.</param>
        public abstract Task<StorageFile> ShowDialogCore(TView view);

        private void Customize()
        {
            if (CustomizeDelegate is not null)
            {
                CustomizeDelegate.Invoke(_view, _dataContext.DialogControl);
            }
            else
            {
                CustomizeCore(_view, _dataContext.DialogControl);
            }
        }

        private bool? HandleResult(StorageFile result)
        {
            if (HandleResultDelgate is not null)
            {
                return HandleResultDelgate.Invoke(result, _view, _dataContext.DialogControl);
            }
            else
            {
                return HandleResultCore(result, _view, _dataContext.DialogControl);
            }
        }
    }
}
