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
    /// <summary>
    /// Provides a standard implementation of <see cref="IDialogModule{T}"/> for WinUI 3 <see cref="FileOpenPicker"/> classes.
    /// </summary>
    public class OpenFileDialogModule : IDialogModule<OpenFileDialogProperties>
    {
        private Window _mainWindow;
        private FileOpenPicker _view;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFileDialogModule"/> class.
        /// </summary>
        /// <param name="mainWindow">Application main window.</param>
        /// <param name="view">Technology-specific dialog object.</param>
        public OpenFileDialogModule(Window mainWindow, FileOpenPicker view)
        {
            _view = view;
            _mainWindow = mainWindow;
        }

        /// <inheritdoc/>
        public object? View => _view;

        /// <inheritdoc/>
        public object? Parent => _mainWindow;

        /// <inheritdoc/>
        public OpenFileDialogProperties Properties { get; set; } = new();

        /// <inheritdoc/>
        public event EventHandler? Opening;
        
        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <summary>
        /// Satisfies <see cref="IDisposable"/> interface.
        /// In this particular case no resources are explicitly freed.
        /// </summary>
        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
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

        protected virtual Window FindParent(object host)
        {
            return _mainWindow;
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
    }
}
