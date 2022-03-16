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

namespace MochaCoreWinUI.DialogsEx
{
    /// <summary>
    /// Provides a standard implementation of <see cref="IDialogModule{T}"/> for WinUI 3 <see cref="FolderPicker"/> class.
    /// </summary>
    public class BrowseFolderDialogModule : BrowseBaseDialogModule<FolderPicker, StorageFolder, BrowseFolderDialogProperties>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseFolderDialogModule"/> class.
        /// </summary>
        /// <param name="mainWindow">Main application window.</param>
        public BrowseFolderDialogModule(Window mainWindow) : base(mainWindow, new BrowseFolderDialogProperties(), new FolderPicker()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseFolderDialogModule"/> class.
        /// </summary>
        /// <param name="mainWindow">Main application window.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        public BrowseFolderDialogModule(Window mainWindow, BrowseFolderDialogProperties properties) : base(mainWindow, properties, new FolderPicker()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseFolderDialogModule"/> class.
        /// </summary>
        /// <param name="mainWindow">Main application window.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        /// <param name="view">Underlying technology-specific dialog object.</param>
        public BrowseFolderDialogModule(Window mainWindow, BrowseFolderDialogProperties properties, FolderPicker view) : base(mainWindow, properties, view) { }

        /// <inheritdoc/>
        protected override void ApplyPropertiesCore(FolderPicker dialog, BrowseFolderDialogProperties properties)
        {
            dialog.FileTypeFilter.Add("*");
        }

        /// <inheritdoc/>
        protected override bool? HandleResultCore(FolderPicker dialog, StorageFolder result, BrowseFolderDialogProperties properties)
        {
            if (result is null)
            {
                return false;
            }

            properties.SelectedPath = result.Path;
            return true;
        }

        /// <inheritdoc/>
        protected override Task<StorageFolder> ShowDialogCore(FolderPicker dialog, Window parent)
        {
            return dialog.PickSingleFolderAsync().AsTask();
        }
    }
}
