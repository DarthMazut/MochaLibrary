using Microsoft.UI.Xaml;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace MochaWinUI.Dialogs
{
    /// <summary>
    /// Provides a standard implementation of <see cref="IDialogModule{T}"/> for WinUI 3 <see cref="FileSavePicker"/> class.
    /// </summary>
    public class SaveFileDialogModule : BrowseBaseDialogModule<FileSavePicker, StorageFile, SaveFileDialogProperties>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileDialogModule"/> class.
        /// </summary>
        /// <param name="mainWindow">Main application window.</param>
        public SaveFileDialogModule(Window mainWindow) : base(mainWindow, new SaveFileDialogProperties(), new FileSavePicker()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileDialogModule"/> class.
        /// </summary>
        /// <param name="mainWindow">Main application window.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        public SaveFileDialogModule(Window mainWindow, SaveFileDialogProperties properties) : base(mainWindow, properties, new FileSavePicker()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileDialogModule"/> class.
        /// </summary>
        /// <param name="mainWindow">Main application window.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        /// <param name="view">Technology-specific dialog object.</param>
        public SaveFileDialogModule(Window mainWindow, SaveFileDialogProperties properties, FileSavePicker view) : base(mainWindow, properties, view) { }

        /// <inheritdoc/>
        protected override void ApplyPropertiesCore(FileSavePicker dialog, SaveFileDialogProperties properties)
        {
            if (!Properties.Filters.Any())
            {
                dialog.FileTypeChoices.Add(string.Empty, new List<string> { "." });
                return;
            }

            foreach (ExtensionFilter filter in Properties.Filters)
            {
                dialog.FileTypeChoices.Add(filter.Name, filter.Extensions.Select(e => $".{e}").ToList());
            }

            dialog.SuggestedStartLocation = MapSpecialFolderToLocationId(properties.TryGetInitialDirectoryAsSpecialFolder());
            if (DialogIdentifier is not null)
            {
                dialog.SettingsIdentifier = DialogIdentifier;
            }
        }

        /// <inheritdoc/>
        protected override bool? HandleResultCore(FileSavePicker dialog, StorageFile result, SaveFileDialogProperties properties)
        {
            if (result is not null)
            {
                properties.SelectedPath = result.Path;
                return true;
            }

            return null;
        }

        /// <inheritdoc/>
        protected override Task<StorageFile> ShowDialogCore(FileSavePicker dialog)
        {
            return dialog.PickSaveFileAsync().AsTask();
        }
    }

    /*
    /// <summary>
    /// Provides a standard implementation of <see cref="IDialogModule{T}"/> for WinUI 3 <see cref="FileSavePicker"/> class.
    /// </summary>
    public class SaveFileDialogModule : IDialogModule<SaveFileDialogProperties>
    {
        private readonly Window _mainWindow;
        private readonly FileSavePicker _view;
        private object? _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileDialogModule"/> class.
        /// </summary>
        /// <param name="mainWindow">Application main window.</param>
        /// <param name="view">Technology-specific dialog object.</param>
        public SaveFileDialogModule(Window mainWindow, FileSavePicker view)
        {
            _mainWindow = mainWindow;
            _view = view;
        }

        /// <inheritdoc/>
        public object? View => _view;

        /// <inheritdoc/>
        public object? Parent => _parent;

        /// <inheritdoc/>
        public SaveFileDialogProperties Properties { get; set; } = new();

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

            bool? result;
            result = HandleSelectionResult(await _view.PickSaveFileAsync());

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

        private void ApplyProperties()
        {
            if (!Properties.Filters.Any())
            {
                _view.FileTypeChoices.Add(string.Empty, new List<string> { "." });
                return;
            }

            foreach (ExtensionFilter filter in Properties.Filters)
            {
                _view.FileTypeChoices.Add(filter.Name, filter.Extensions.Select(e => $".{e}").ToList());
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

        // Workaround for bug https://github.com/microsoft/WindowsAppSDK/issues/466
        private void WorkaroundForBug466()
        {
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(Parent);
            WinRT.Interop.InitializeWithWindow.Initialize(_view, hwnd);
        }
    }
    */
}
