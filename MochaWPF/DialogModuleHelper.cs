using Mocha.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MochaWPF
{
    /// <summary>
    /// Provides useful methods for <see cref="IDialogModule"/> implementation process.
    /// </summary>
    public static class DialogModuleHelper
    {
        /// <summary>
        /// Returns parent <see cref="Window"/> based on value from <see cref="DialogParameters"/>.
        /// </summary>
        /// <param name="application">A reference to WPF <see cref="Application"/> object.</param>
        /// <param name="dataContext">Dialog backend containing <see cref="DialogParameters"/>.</param>
        public static Window GetParentWindow(Application application, IDialog dataContext)
        {
            Window parent = null;

            application.Dispatcher.Invoke(() =>
            {
                List<IDialogModule> modules = DialogManager.GetActiveDialogs();
                IDialog parentDialog = dataContext.DialogParameters.Parent;

                IDialogModule parentModule = modules.Where(m => m.DataContext == parentDialog).FirstOrDefault();

                foreach (Window window in application.Windows)
                {
                    if (window == parentModule?.View)
                    {
                        parent = window;
                        return;
                    }
                }

                parent = application.Windows[0];
            });

            return parent;
        }
    }
}
