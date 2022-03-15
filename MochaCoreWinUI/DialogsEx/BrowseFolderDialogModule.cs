using Microsoft.UI.Xaml;
using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT;

namespace MochaCoreWinUI.DialogsEx
{
    public class BrowseFolderDialogModule : BrowseBaseDialogModule<FolderPicker, StorageFolder, BrowseFolderDialogProperties>
    {
        public BrowseFolderDialogModule(Window mainWindow) : base(mainWindow, new BrowseFolderDialogProperties(), new FolderPicker()) { }

        public BrowseFolderDialogModule(Window mainWindow, BrowseFolderDialogProperties properties) : base(mainWindow, properties, new FolderPicker()) { }

        public BrowseFolderDialogModule(Window mainWindow, BrowseFolderDialogProperties properties, FolderPicker view) : base(mainWindow, properties, view) { }

        protected override void ApplyPropertiesCore(FolderPicker dialog, BrowseFolderDialogProperties properties)
        {
            dialog.FileTypeFilter.Add("*");
        }

        protected override bool? HandleResultCore(FolderPicker dialog, StorageFolder result, BrowseFolderDialogProperties properties)
        {
            if (result is null)
            {
                return false;
            }

            properties.SelectedPath = result.Path;
            return true;
        }

        protected override Task<StorageFolder> ShowDialogCore(FolderPicker dialog, Window parent)
        {
            return dialog.PickSingleFolderAsync().AsTask();
        }
    }


    /*
    /// <summary>
    /// Provides a standard implementation of <see cref="IDialogModule{T}"/> for WinUI 3 <see cref="FolderPicker"/> class.
    /// </summary>
    public class BrowseFolderDialogModule : IDialogModule<BrowseFolderDialogProperties>
    {
        private Window _mainWindow;
        private FolderPicker _view;
        private object? _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseFolderDialogModule"/> class.
        /// </summary>
        /// <param name="mainWindow">Application main window.</param>
        /// <param name="view">Technology-specific counterpart of this module.</param>
        public BrowseFolderDialogModule(Window mainWindow, FolderPicker view)
        {
            _view = view;
            _mainWindow = mainWindow;
        }

        /// <inheritdoc/>
        public object? View => _view;

        /// <inheritdoc/>
        public object? Parent => _parent;

        /// <inheritdoc/>
        public BrowseFolderDialogProperties Properties { get; set; } = new();

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
            Opening?.Invoke(this, EventArgs.Empty);
            _parent = FindParent(host);
            WorkaroundForBug466();

            bool? result = HandleSelectionResult(await _view.PickSingleFolderAsync());

            _parent = null;
            Closed?.Invoke(this, EventArgs.Empty);
            return result;
        }

        /// <summary>
        /// Tries to find technology-specific parent of provided technology-independent element.
        /// </summary>
        /// <param name="host">Technology-independent element, which technology-specific parent is to be found.</param>
        protected virtual Window FindParent(object host)
        {
            // This has to be done after windowing API is released.
            // Nothing can be done at this point. 
            return _mainWindow;
        }

        // Workaround for bug https://github.com/microsoft/WindowsAppSDK/issues/466
        private void WorkaroundForBug466()
        {
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(_parent);
            WinRT.Interop.InitializeWithWindow.Initialize(_view, hwnd);
        }

        private void ApplyProperties()
        {
            _view.FileTypeFilter.Add("*");
        }

        private bool? HandleSelectionResult(StorageFolder storageFolder)
        {
            if (storageFolder is not null)
            {
                Properties.SelectedPath = storageFolder.Path;
                return true;
            }

            return false;
        }
    }
    */
}
