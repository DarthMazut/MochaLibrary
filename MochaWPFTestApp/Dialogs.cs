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
        public static IDialogModule OpenDialog => DialogManager.GetDialog(DialogsIDs.OpenDialog);
    }

    static class DialogsIDs
    {
        public static string OpenDialog => "OpenDialog";
    }
}
