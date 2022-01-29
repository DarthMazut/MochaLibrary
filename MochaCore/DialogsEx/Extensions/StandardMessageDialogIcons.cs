using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx.Extensions
{
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

        public static string Information => "Info";

        public static string Question => "Question";
    }
}
