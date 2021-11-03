using MochaCore.Dialogs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace MochaCoreWinUI.Dialogs
{
    /// <summary>
    /// Provides implementation of 
    /// </summary>
    public class SaveFileDialogModule : FileDialogModuleBase<FileSavePicker, FileDialogControl>
    {
        public SaveFileDialogModule(FileSavePicker view) : base(view) { }

        public override Task<StorageFile> ShowDialogCore(FileSavePicker view)
        {
            return view.PickSaveFileAsync().AsTask();
        }

        protected override void CustomizeCore(FileSavePicker view, FileDialogControl dialogControl)
        {
            view.DefaultFileExtension = dialogControl.DefaultExtension;
            // TODO: Handle rest of values...
        }

        protected override bool? HandleResultCore(StorageFile result, FileSavePicker view, FileDialogControl dialogControl)
        {
            dialogControl.SelectedPath = result.Path;

            return result is not null ? true : null;
        }
    }
}
