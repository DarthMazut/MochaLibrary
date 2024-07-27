using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Dialogs
{
    public static class AppDialogs
    {
        public static AppDialog<ICustomDialogModule<StandardMessageDialogProperties>> StandardMessageDialog { get; }
            = new("MessageDialog");
    }

    public class AppDialog<T>(string id) where T : IDialogModule
    {
        public string Id { get; } = id;

        public T Module => (T)DialogManager.RetrieveDialog(Id);
    }
}

