using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Mocha.Dialogs;

namespace MochaWPF.Prototypes
{
    class MyTestDialog : CustomDialogModulePrototype
    {
        public MyTestDialog(Application application, Window window, IDialog dataContext) : base(application, window, dataContext)
        {
        }
    }
}
