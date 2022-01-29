using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx.Extensions
{
    /// <summary>
    /// Provides a set of properties for standard dialog.
    /// </summary>
    public class StandardMessageDialogProperties
    {
        public StandardMessageDialogProperties(string title, string message)
        {
            Title = title;
            Message = message;
        }

        /// <summary>
        /// Title for standard message dialog.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Message content for standard message dialog.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Specifies icon for standard message dialog.
        /// <see langword="Null"/> means icon won't show up.
        /// <para>Check <see cref="StandardMessageDialogIcons"/> class for predefined icons identifiers.</para>
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// Label for confirmation button. This button is always present.
        /// </summary>
        public string ConfirmationButtonText { get; set; } = "OK";

        /// <summary>
        /// Label for decline button. This button is optional. 
        /// <see langword="Null"/> means this button won't be present.
        /// </summary>
        public string? DeclineButtonText { get; set; }

        /// <summary>
        /// Label for cancellation button. This button is optional. 
        /// <see langword="Null"/> means this button is not present.
        /// </summary>
        public string? CancelButtonText { get; set; }
    }


}
