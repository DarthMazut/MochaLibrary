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
    /// Provides a standard implementation of <see cref="IDialogModule{T}"/> for WinUI 3 <see cref="FileOpenPicker"/> class.
    /// </summary>
    public class OpenFileDialogModule : IDialogModule<OpenFileDialogProperties>
    {
        private Window _mainWindow;
        private FileOpenPicker _view;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFileDialogModule"/> class.
        /// </summary>
        /// <param name="mainWindow">Application main window.</param>
        public OpenFileDialogModule(Window mainWindow) : this(mainWindow, new OpenFileDialogProperties(), new FileOpenPicker()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFileDialogModule"/> class.
        /// </summary>
        /// <param name="mainWindow">Application main window.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        public OpenFileDialogModule(Window mainWindow, OpenFileDialogProperties properties) : this(mainWindow, properties, new FileOpenPicker()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFileDialogModule"/> class.
        /// </summary>
        /// <param name="mainWindow">Application main window.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        /// <param name="view">Technology-specific dialog object.</param>
        public OpenFileDialogModule(Window mainWindow, OpenFileDialogProperties properties, FileOpenPicker view)
        {
            _view = view;
            _mainWindow = mainWindow;
            Properties = properties;

            ApplyProperties = ApplyPropertiesCore;
            ShowDialogForSingleFile = ShowDialogForSingleFileCore;
            ShowDialogForMultipleFiles = ShowDialogForMultipleFilesCore;
            HandleSingleResult = HandleSingleResultCore;
            HandleMultipleResults = HandleMultipleResultsCore;
            FindParent = FindParentCore;
            DisposeDialog = DisposeDialogCore;
        }

        /// <inheritdoc/>
        public object? View => _view;

        /// <inheritdoc/>
        public OpenFileDialogProperties Properties { get; set; }

        /// <inheritdoc/>
        public event EventHandler? Opening;
        
        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <summary>
        /// Applies <see cref="Properties"/> values to technology-specific dialog object.
        /// </summary>
        public Action<FileOpenPicker, OpenFileDialogProperties> ApplyProperties { get; set; }

        /// <summary>
        /// Handles technology-specific process of dialog show for single file to be opened.
        /// </summary>
        public Func<FileOpenPicker, Task<StorageFile>> ShowDialogForSingleFile { get; set; }

        /// <summary>
        /// Handles technology-specific process of dialog show for multiple files to be opened.
        /// </summary>
        public Func<FileOpenPicker, Task<IReadOnlyList<StorageFile>>> ShowDialogForMultipleFiles { get; set; }

        /// <summary>
        /// Translates technology-specific dialog result into technolog-independant value for single file.
        /// Sets suitable properties in <see cref="Properties"/> if required.
        /// </summary>
        public Func<FileOpenPicker, StorageFile, OpenFileDialogProperties, bool?> HandleSingleResult { get; set; }

        /// <summary>
        /// Translates technology-specific dialog result into technolog-independant value for multiple files.
        /// Sets suitable properties in <see cref="Properties"/> if required.
        /// </summary>
        public Func<FileOpenPicker, IReadOnlyList<StorageFile>, OpenFileDialogProperties, bool?> HandleMultipleResults { get; set; }

        /// <summary>
        /// Handles the process of search for parent <see cref="Window"/> for technology-specific dialog object.
        /// </summary>
        public Func<object, Window> FindParent { get; set; }

        /// <summary>
        /// Allows for providing a custom code to be executed while this object is being disposed of.
        /// Use this delegate when there are disposable resources within your custom <see cref="Properties"/> object.
        /// </summary>
        public Action<OpenFileDialogModule> DisposeDialog { get; set; }

        /// <summary>
        /// Diffrientiates between diffrent pickers.
        /// </summary>
        public string? DialogIdentifier { get; init; }

        /// <inheritdoc/>
        public void Dispose()
        {
            DisposeDialog.Invoke(this);
            Disposed?.Invoke(this, EventArgs.Empty);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public async Task<bool?> ShowModalAsync(object host)
        {
            ApplyProperties.Invoke(_view, Properties);
            Opening?.Invoke(this, EventArgs.Empty);
            WorkaroundForBug466(FindParent(host));

            bool? result;
            if (Properties.MultipleSelection)
            {
                result = HandleMultipleResults.Invoke(_view, await ShowDialogForMultipleFiles(_view), Properties);
            }
            else
            {
                result = HandleSingleResult(_view, await ShowDialogForSingleFile(_view), Properties);
            }

            Closed?.Invoke(this, EventArgs.Empty);
            return result;
        }

        /// <summary>
        /// Applies <see cref="Properties"/> values to technology-specific dialog object.
        /// </summary>
        /// <param name="dialog">Technology-specific dialog object.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        protected virtual void ApplyPropertiesCore(FileOpenPicker dialog, OpenFileDialogProperties properties)
        {
            if (!properties.Filters.Any())
            {
                dialog.FileTypeFilter.Add("*");
                return;
            }

            foreach (ExtensionFilter filter in properties.Filters)
            {
                foreach (string ext in filter.Extensions)
                {
                    dialog.FileTypeFilter.Add($".{ext}");
                }
            }

            dialog.SuggestedStartLocation = MapSpecialFolderToLocationId(properties.TryGetInitialDirectoryAsSpecialFolder());
            if (DialogIdentifier is not null)
            {
                dialog.SettingsIdentifier = DialogIdentifier;
            }
        }

        /// <summary>
        /// Handles technology-specific process of showing <see cref="FileOpenPicker"/> for single file to be picked.
        /// </summary>
        /// <param name="dialog">Dialog to be displayed.</param>
        protected virtual Task<StorageFile> ShowDialogForSingleFileCore(FileOpenPicker dialog)
        {
            return dialog.PickSingleFileAsync().AsTask();
        }

        /// <summary>
        /// Handles technology-specific process of showing <see cref="FileOpenPicker"/> for multiple files to be picked.
        /// </summary>
        /// <param name="dialog">Dialog to be displayed.</param>
        protected virtual Task<IReadOnlyList<StorageFile>> ShowDialogForMultipleFilesCore(FileOpenPicker dialog)
        {
            return dialog.PickMultipleFilesAsync().AsTask();
        }

        /// <summary>
        /// Translates technology-specific dialog result into technolog-independant value.
        /// Sets suitable properties in <see cref="Properties"/> if required.
        /// </summary>
        /// <param name="dialog">Technology-specific dialog object.</param>
        /// <param name="storageFile">Technology-specific result of dialog interaction.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        protected virtual bool? HandleSingleResultCore(FileOpenPicker dialog, StorageFile storageFile, OpenFileDialogProperties properties)
        {
            if (storageFile is not null)
            {
                properties.SelectedPaths.Add(storageFile.Path);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Translates technology-specific dialog result into technolog-independant value.
        /// Sets suitable properties in <see cref="Properties"/> if required.
        /// </summary>
        /// <param name="dialog">Technology-specific dialog object.</param>
        /// <param name="storageFiles">Technology-specific result of dialog interaction.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        protected virtual bool? HandleMultipleResultsCore(FileOpenPicker dialog, IReadOnlyList<StorageFile> storageFiles, OpenFileDialogProperties properties)
        {
            if (storageFiles.Any())
            {
                properties.SelectedPaths = storageFiles.Select(f => f.Path).ToList();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to find technology-specific parent of provided technology-independent element.
        /// </summary>
        /// <param name="host">Technology-independent element, which technology-specific parent is to be found.</param>
        protected virtual Window FindParentCore(object host)
        {
            // This has to be done after windowing API is released.
            // Nothing can be done at this point. 
            return _mainWindow;
        }

        /// <summary>
        /// Allows for providing a custom code to be executed while this object is being disposed of.
        /// Override this when there are disposable resources within your custom <see cref="Properties"/> object.
        /// </summary>
        /// <param name="module">Module that's being disposed.</param>
        protected virtual void DisposeDialogCore(OpenFileDialogModule module) { }

        // Workaround for bug https://github.com/microsoft/WindowsAppSDK/issues/466
        private void WorkaroundForBug466(Window parent)
        {
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(parent);
            WinRT.Interop.InitializeWithWindow.Initialize(_view, hwnd);
        }

        private static PickerLocationId MapSpecialFolderToLocationId(Environment.SpecialFolder? folder)
        {
            Dictionary<Environment.SpecialFolder, PickerLocationId> locationMap = new()
            {
                {Environment.SpecialFolder.MyDocuments, PickerLocationId.DocumentsLibrary},
                {Environment.SpecialFolder.MyComputer, PickerLocationId.ComputerFolder},
                {Environment.SpecialFolder.Desktop, PickerLocationId.Desktop},
                {Environment.SpecialFolder.MyMusic, PickerLocationId.MusicLibrary},
                {Environment.SpecialFolder.MyPictures, PickerLocationId.PicturesLibrary},
                {Environment.SpecialFolder.MyVideos, PickerLocationId.VideosLibrary}
            };

            if (folder.HasValue && locationMap.TryGetValue(folder.Value, out PickerLocationId locationId))
            {
                return locationId;
            }

            return PickerLocationId.Unspecified;
        }
    }
}
