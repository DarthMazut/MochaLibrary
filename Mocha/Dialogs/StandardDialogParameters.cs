using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Dialogs
{
    /// <summary>
    /// Contains standarized keys for dialog parameters.
    /// </summary>
    public static class StandardDialogParameters
    {
        /// <summary>
        /// Path selected as a result of interaction with file dialog.
        /// </summary>
        public static string SelectedPath => $"__MochaLibParam__{nameof(SelectedPath)}";

        /// <summary>
        /// Defines a filter for Open/Save dialog.
        /// </summary>
        public static string Filter => $"__MochaLibParam__{nameof(Filter)}";

        /// <summary>
        /// Sets initial directory for Open/Save dialog.
        /// </summary>
        public static string InitialDirectory => $"__MochaLibParam__{nameof(InitialDirectory)}";

        /// <summary>
        /// Extension added automatically in case none was specified.
        /// </summary>
        public static string DefaultExtension => $"__MochaLibParam__{nameof(DefaultExtension)}";

        /// <summary>
        /// Message for displaying dialog.
        /// </summary>
        public static string Message => $"__MochaLibParam__{nameof(Message)}";

        /// <summary>
        /// Icon for displaying dialog.
        /// </summary>
        public static string Icon => $"__MochaLibParam__{nameof(Icon)}";

        /// <summary>
        /// Defines a predefined buttons set for displaying dialog.
        /// </summary>
        public static string PredefinedButtons => $"__MochaLibParam__{nameof(PredefinedButtons)}";
    }
}
