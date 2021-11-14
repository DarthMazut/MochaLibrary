using Microsoft.UI.Xaml;
using MochaCore.Dialogs.Extensions;
using MochaCore.Dialogs;
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
    /// Provides a standard implementation of <see cref="IDialogModule"/> for WinUI 3 <see cref="FileOpenPicker"/> classes.
    /// </summary>
    public class OpenFileDialogModule : FileDialogModuleBase<FileOpenPicker, FileDialogControl>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFileDialogModule"/> class.
        /// </summary>
        /// <param name="parentWindow">Parent window.</param>
        /// <param name="view">Technology-specific dialog object.</param>
        public OpenFileDialogModule(Window parentWindow, FileOpenPicker view) : base(parentWindow, view) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        public override Task<StorageFile> ShowDialogCore(FileOpenPicker view)
        {
            return view.PickSingleFileAsync().AsTask();
        }

        /// <inheritdoc/>
        protected override void CustomizeCore(FileOpenPicker view, FileDialogControl dialogControl)
        {
            view.FileTypeFilter.Add("*");
        }

        /// <inheritdoc/>
        protected override bool? HandleResultCore(StorageFile result, FileOpenPicker view, FileDialogControl dialogControl)
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
