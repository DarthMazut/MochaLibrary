using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mocha.Dialogs
{
    /// <summary>
    /// Contains predefined keys for dialog parameters dictionary.
    /// </summary>
    public class DialogParameters
    {
        /// <summary>
        /// A key for a standard <see langword="Owner"/> value.
        /// </summary>
        public static string Owner => $"Mocha__{MethodBase.GetCurrentMethod().Name}";

        /// <summary>
        /// A key for a standard <see langword="Title"/> value.
        /// </summary>
        public static string Title => $"Mocha__{MethodBase.GetCurrentMethod().Name}";

        /// <summary>
        /// A key for a standard <see langword="Caption"/> value.
        /// </summary>
        public static string Caption => $"Mocha__{MethodBase.GetCurrentMethod().Name}";

        /// <summary>
        /// A key for a standard <see langword="Buttons"/> value.
        /// </summary>
        public static string SimpleButtons => $"Mocha__{MethodBase.GetCurrentMethod().Name}";

        /// <summary>
        /// A key for a standard <see langword="Icon"/> value.
        /// </summary>
        public static string Icon => $"Mocha__{MethodBase.GetCurrentMethod().Name}";
    }
}
