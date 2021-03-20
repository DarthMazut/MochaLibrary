using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mocha.Dialogs
{
    /// <summary>
    /// Contains a delegates which can be set by <see cref="IDialog"/> implementation.
    /// Thos delegates are then invoked by <see cref="IDialogModule"/> at proper time.
    /// </summary>
    public class DialogEvents
    {
        /// <summary>
        /// Invoked when dialog is about to close. Use <see cref="CancelEventArgs"/> object 
        /// to prevent dialog from closing.
        /// </summary>
        public Action<CancelEventArgs> OnClosing { get; set; }
    }
}
