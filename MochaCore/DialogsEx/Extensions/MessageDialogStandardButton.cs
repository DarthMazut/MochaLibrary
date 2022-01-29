using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx.Extensions
{
    /// <summary>
    /// Defines a standard buttons for message dialogs.
    /// </summary>
    public enum MessageDialogStandardButton
    {
        /// <summary>
        /// Default confirmation button. This button is always present.
        /// </summary>
        ConfirmButton,

        /// <summary>
        /// Negation button. This button is optional.
        /// </summary>
        DeclineButton,

        /// <summary>
        /// Cancel button (optional).
        /// </summary>
        CancelButton
    }
}
