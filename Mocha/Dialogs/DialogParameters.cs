using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mocha.Dialogs
{
    /// <summary>
    /// Contains predefined, standard parameters for <see cref="IDialogModule"/> customization.
    /// </summary>
    public class DialogParameters
    {
        /// <summary>
        /// Title for displaying dialog.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Message for displaying dialog.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Icon for displaying dialog.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Defines a predefined buttons set for displaying dialog.
        /// </summary>
        public string PredefinedButtons { get; set; }

        /// <summary>
        /// Name of a parent element which called a dialog parametrized by this instance.
        /// </summary>
        public string ParentName { get; set; }
    }
}
