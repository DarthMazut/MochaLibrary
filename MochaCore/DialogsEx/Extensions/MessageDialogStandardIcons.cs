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
    public static class MessageDialogStandardIcons
    {
        public static string Error => "Error";

        public static string Warning => "Warning";

        public static string Information => "Info";

        public static string Question => "Error";

        /// <summary>
        /// No icon should be displayed. Such behaviour should be achieved by providing an empty string or <see langword="null"/>.
        /// </summary>
        public static string None => string.Empty;
    }
}
