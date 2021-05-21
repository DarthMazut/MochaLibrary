using Mocha.Dialogs;
using Mocha.Dialogs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWPFTestApp
{
    static class Dialogs
    {
        public static IDialogModule CustomDialog1 => DialogManager.GetDialog(DialogsIDs.CustomDialog1);
        public static IDialogModule<StandardDialogControl> MsgBoxDialog => (IDialogModule<StandardDialogControl>)DialogManager.GetDialog(DialogsIDs.MsgBoxDialog);
        public static IDialogModule<FileDialogControl> OpenDialog => (IDialogModule<FileDialogControl>)DialogManager.GetDialog(DialogsIDs.OpenDialog);
    }

    static class DialogsIDs
    {
        public static string CustomDialog1 => "CustomDialog1";
        public static string MsgBoxDialog => "MsgBoxDialog";
        public static string OpenDialog => "OpenDialog";
    }
}
