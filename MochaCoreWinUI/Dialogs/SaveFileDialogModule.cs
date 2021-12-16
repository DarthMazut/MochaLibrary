using MochaCore.Dialogs.Extensions;
using MochaCore.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml;

namespace MochaCoreWinUI.Dialogs
{
    /// <summary>
    /// Provides a standard implementation of <see cref="IDialogModule"/> for WinUI 3 <see cref="FileSavePicker"/> classes.
    /// </summary>
    public class SaveFileDialogModule : FileDialogModuleBase<FileSavePicker, FileDialogControl>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileDialogModule"/> class.
        /// </summary>
        /// <param name="parentWindow">Parent window.</param>
        /// <param name="view">Technology-specific dialog object.</param>
        public SaveFileDialogModule(Window parentWindow, FileSavePicker view) : base(parentWindow, view) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        public override Task<StorageFile> ShowDialogCore(FileSavePicker view)
        {
            return view.PickSaveFileAsync().AsTask();
        }

        /// <inheritdoc/>
        protected override void CustomizeCore(FileSavePicker view, FileDialogControl dialogControl)
        {
            view.FileTypeChoices.Add("Default", new List<string> { "." });
        }

        /// <inheritdoc/>
        protected override bool? HandleResultCore(StorageFile result, FileSavePicker view, FileDialogControl dialogControl)
        {
            if (result is not null)
            {
                dialogControl.SelectedPath = result.Path;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
