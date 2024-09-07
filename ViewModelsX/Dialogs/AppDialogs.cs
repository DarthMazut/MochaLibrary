using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelsX.Pages.Dialogs;
using ViewModelsX.Pages.Notfifications.Dialogs;

namespace ViewModelsX.Dialogs
{
    public static class AppDialogs
    {
        public static AppDialog<IDialogModule<SaveFileDialogProperties>> SystemSaveDialog { get; }
            = new("SystemSaveDialog");

        public static AppDialog<IDialogModule<OpenFileDialogProperties>> SystemOpenDialog { get; }
            = new("SystemOpenDialog");

        public static AppDialog<IDialogModule<BrowseFolderDialogProperties>> SystemBrowseDialog { get; }
            = new("SystemBrowseDialog");

        public static AppDialog<ICustomDialogModule<CreateDialogDialogProperties>> CreateDialogDialog { get; }
            = new("CreateDialogDialog");

        public static AppDialog<ICustomDialogModule<StandardMessageDialogProperties>> StandardMessageDialog { get; }
            = new("MessageDialog");

        public static AppDialog<IDialogModule<OpenFileDialogProperties>> StandardOpenFileDialog { get; }
            = new("StandardOpenFileDialog");

        public static AppDialog<ICustomDialogModule<AddSelectableItemDialogProperties>> AddSelectableItemDialog { get; }
            = new("AddSelectableItemDialog");
    }

    public class AppDialog<T>(string id) where T : IDialogModule
    {
        public string Id { get; } = id;

        public T Module => (T)DialogManager.RetrieveDialog(Id);
    }
}

