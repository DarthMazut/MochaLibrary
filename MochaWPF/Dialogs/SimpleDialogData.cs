using Mocha.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWPF.Dialogs
{
    internal class SimpleDialogData : IDialog
    {
        public DialogControl DialogControl { get; }

        public SimpleDialogData()
        {
            DialogControl = new DialogControl(this);
        }
    }
}
