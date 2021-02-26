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
        /// Parent element which called a dialog parametrized by this instance.
        /// Setting this to <see langword="null"/> means MainWindow is consider a parent.
        /// </summary>
        public IDialog Parent { get; set; }

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

        
    }
}
