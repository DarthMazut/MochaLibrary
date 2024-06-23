using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs.Extensions
{
    /// <summary>
    /// Provides a set of properties for standard dialog.
    /// </summary>
    public class StandardMessageDialogProperties
    {
        /// <summary>
        /// Title for standard message dialog.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Message content for standard message dialog.
        /// </summary>
        public string Message { get; set; } = string.Empty;

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


    /// <summary>
    /// Provides identifiers of standard icons for message dialogs.
    /// </summary>
    public static class StandardMessageDialogIcons
    {
        /// <summary>
        /// Returns a string interpreted as an error icon. 
        /// </summary>
        public static string Error => "Error";

        /// <summary>
        /// Returns a string interpreted as a warning icon. 
        /// </summary>
        public static string Warning => "Warning";

        /// <summary>
        /// Returns a string interpreted as a information icon. 
        /// </summary>
        public static string Information => "Info";

        /// <summary>
        /// Returns a string interpreted as a question icon.
        /// </summary>
        public static string Question => "Question";
    }
}
