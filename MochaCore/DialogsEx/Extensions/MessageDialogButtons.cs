using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx.Extensions
{
    /// <summary>
    /// Defines a buttons for messages dialogs.
    /// </summary>
    public class MessageDialogButtons
    {
        /// <summary>
        /// Label for confirmation button. This button is always present.
        /// </summary>
        public string ConfirmationButton { get; set; } = "OK";

        /// <summary>
        /// Label for decline button. This button is optional. 
        /// Empty string or <see langword="null"/> means this button is not present.
        /// </summary>
        public string? DeclineButton { get; set; }

        /// <summary>
        /// Label for cancellation button. This button is optional. 
        /// Empty string or <see langword="null"/> means this button is not present.
        /// </summary>
        public string? CancelButton { get; set; }
    }
}
