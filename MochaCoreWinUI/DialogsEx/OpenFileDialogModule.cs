using Microsoft.UI.Xaml;
using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT;

namespace MochaCoreWinUI.DialogsEx
{
    public class OpenFileDialogModule : IDialogModule<OpenFileDialogProperties>
    {
        private Window _mainWindow;
        private FileOpenPicker _view;

        public OpenFileDialogModule(Window mainWindow, FileOpenPicker view)
        {
            _view = view;
            _mainWindow = mainWindow;
        }

        public object? View => _view;

        public object? Parent => _mainWindow;

        public OpenFileDialogProperties Properties { get; set; } = new();

        public event EventHandler? Opening;
        public event EventHandler? Closed;
        public event EventHandler? Disposed;

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public async Task<bool?> ShowModalAsync(object host)
        {
            ApplyProperties();
            // Workaround for bug https://github.com/microsoft/WindowsAppSDK/issues/466
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(_mainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(_view, hwnd);

            Opening?.Invoke(this, EventArgs.Empty);
            bool? result = default;
            if (Properties.MultipleSelection)
            {
                result = HandleMultipleSelectionResult(await _view.PickMultipleFilesAsync());
            }
            else
            {
                result = HandleSelectionResult(await _view.PickSingleFileAsync());
            }
            Closed?.Invoke(this, EventArgs.Empty);
            return result;
        }

        private void ApplyProperties()
        {
            if (!Properties.Filters.Any())
            {
                _view.FileTypeFilter.Add("*");
                return;
            }

            foreach (ExtensionFilter filter in Properties.Filters)
            {
                foreach (string ext in filter.Extensions)
                {
                    _view.FileTypeFilter.Add($".{ext}");
                }
            }
        }

        private bool? HandleSelectionResult(StorageFile storageFile)
        {
            if (storageFile is not null)
            {
                Properties.SelectedPath = storageFile.Path;
                return true;
            }

            return false;
        }

        private bool? HandleMultipleSelectionResult(IReadOnlyList<StorageFile> storageFiles)
        {
            if (storageFiles.Any())
            {
                Properties.SelectedPaths = storageFiles.Select(f => f.Path).ToList();
                return true;
            }
            
            return false;
        }

        protected virtual Window FindParent(object host)
        {
            return _mainWindow;
        }

    }
}
