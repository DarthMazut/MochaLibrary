using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public static class Dialogs
    {
        public static Dialog<IDialogModule<StandardMessageDialogProperties>> MoreInfoDialog { get; } = new("MoreInfoDialog");

        public static Dialog<ICustomDialogModule<DialogProperties>> EditPictureDialog { get; } = new("EditPictureDialog");

        public static Dialog<IDialogModule<OpenFileDialogProperties>> SelectFileDialog { get; } = new("SelectFileDialog");
    }

    public class Dialog<T> where T : IDialogModule
    {
        public Dialog(string id)
        {
            ID = id;
        }

        public string ID { get; }

        public T Module => (T)DialogManager.GetBaseDialog(ID);
    }
}
