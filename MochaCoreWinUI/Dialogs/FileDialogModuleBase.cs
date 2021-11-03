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
    public abstract class FileDialogModuleBase<TView, TControl> : IDialogModule<TControl> where TControl : DialogControl
    {
        private TView _view;
        private IDialog<TControl> _dataContext;
        private bool _isOpen = false;
        private bool _isDisposed = false;

        public FileDialogModuleBase(TView view)
        {
            _view = view;
            _dataContext = new SimpleDialogData<TControl>();
        }

        public object? View => _view;

        public IDialog<TControl> DataContext => _dataContext;
        IDialog IDialogModule.DataContext => _dataContext;

        public bool IsOpen => _isOpen;

        public event EventHandler? Opening;

        public event EventHandler? Closed;

        public event EventHandler? Disposed;

        public event EventHandler? Opened
        {
            add => throw new InvalidOperationException($"{nameof(Opened)} event is not supported by {GetType().Name}.");
            remove => throw new InvalidOperationException($"{nameof(Opened)} event is not supported by {GetType().Name}.");
        }

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

        public void Close()
        {
            throw new InvalidOperationException($"{GetType().Name} does not support Close() method.");
        }

        public void Dispose()
        {
            _dataContext.DialogControl.Dispose();
            Disposed?.Invoke(this, EventArgs.Empty);
        }

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

        public void Show()
        {
            throw new InvalidOperationException($"{GetType().Name} does not support synchronous Show() method.");
        }

        public Task ShowAsync()
        {
            throw new InvalidOperationException($"{GetType().Name} does not support ShowAsync() method.");
        }

        public bool? ShowModal()
        {
            throw new InvalidOperationException($"{GetType().Name} does not support synchronous ShowModal() method.");
        }

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

            Customize();
            Opening?.Invoke(this, EventArgs.Empty);
            _isOpen = true;
            StorageFile result = await ShowDialogCore(_view);
            _isOpen = false;
            Closed?.Invoke(this, EventArgs.Empty);
            return HandleResult(result);
        }

        protected abstract void CustomizeCore(TView view, TControl dialogControl);

        protected abstract bool? HandleResultCore(StorageFile result, TView view, TControl dialogControl);

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
