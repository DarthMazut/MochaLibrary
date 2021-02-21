using Mocha.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWPFTestApp.ViewModels.Dialogs
{
    class StandardDialogModuleViewModel : IDialog
    {
        public bool? DialogResult { get; set; }

        public object DialogValue { get; set; }

        public Dictionary<string, object> Parameters { get; set; }
    }
}
