using Microsoft.UI.Xaml;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace MochaCoreWinUI.Dialogs
{
    public class OpenFileDialogModule : IDialogModule<FileDialogControl>
    {
        private Window _parentWindow;
        private readonly FileOpenPicker _view;
        private readonly FileSavePicker _view2;
        private IDialog<FileDialogControl> _dataContext;
        private bool _isOpen;
        private bool _isDisposed;

        public OpenFileDialogModule(Window parentWindow, FileOpenPicker picker)
        {
            _parentWindow = parentWindow;
            _view = picker;
            SetDataContext(new SimpleDialogData<FileDialogControl>());
        }

        public object? View => _view;

        IDialog IDialogModule.DataContext => _dataContext;

        public IDialog<FileDialogControl> DataContext => _dataContext;

        public bool IsOpen => _isOpen;

        public event EventHandler? Opening;
        public event EventHandler? Opened;
        public event EventHandler<CancelEventArgs>? Closing;
        public event EventHandler? Closed;
        public event EventHandler? Disposed;

        public void Close()
        {
            throw new InvalidOperationException($"{GetType().Name} does not support Close() method.");
        }

        public void Dispose()
        {
            _isDisposed = true;
            _dataContext.DialogControl.Dispose();
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public void SetDataContext(IDialog dialog)
        {
            if (dialog is IDialog<FileDialogControl> typedDialog)
            {
                _dataContext = typedDialog;
                _dataContext.DialogControl.Activate(this);
            }
            else
            {
                throw new ArgumentException($"Data context for {GetType().Name} can only be of type {typeof(IDialog<FileDialogControl>)}");
            }
        }

        public void Show()
        {
            throw new InvalidOperationException($"{GetType().Name} does not support synchronous Show() method.");
        }

        public bool? ShowModal()
        {
            throw new InvalidOperationException($"{GetType().Name} does not support synchronous ShowModal() method.");
        }

        public Task ShowAsync()
        {
            throw new InvalidOperationException($"{GetType().Name} does not support ShowAsync() method.");
        }

        public async Task<bool?> ShowModalAsync()
        {
            if (_isOpen)
            {
                throw new InvalidOperationException("Cannot open already opened dialog.");
            }

            if(_isDisposed)
            {
                throw new InvalidOperationException("Cannot open disposed dialog.");
            }

            // Workaround for bug https://github.com/microsoft/WindowsAppSDK/issues/466
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(_parentWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(_view, hwnd);
            _view.FileTypeFilter.Add("*");

            Opening?.Invoke(this, EventArgs.Empty);
            Customize(_view, _dataContext.DialogControl);
            _isOpen = true;
            StorageFile? pickedFile = await _view.PickSingleFileAsync();
            bool? result = HandleResult(pickedFile, _dataContext.DialogControl);
            _isOpen = false;
            Closed?.Invoke(this, EventArgs.Empty);
            return result;
        }

        private void Customize(FileOpenPicker view, FileDialogControl dialogControl)
        {
            
        }

        private bool? HandleResult(StorageFile pickedFile, FileDialogControl dialogControl)
        {
            dialogControl.DialogResult = pickedFile is not null;
            dialogControl.SelectedPath = pickedFile?.Path;

            return dialogControl.DialogResult;
        }
    }
}
