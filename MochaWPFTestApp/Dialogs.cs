using Mocha.Dialogs;
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
        public static IDialogModule MsgBoxDialog => DialogManager.GetDialog(DialogsIDs.MsgBoxDialog);
        public static IDialogModule OpenDialog => DialogManager.GetDialog(DialogsIDs.OpenDialog);
    }

    static class DialogsIDs
    {
        public static string CustomDialog1 => "CustomDialog1";
        public static string MsgBoxDialog => "MsgBoxDialog";
        public static string OpenDialog => "OpenDialog";
    }
}
