using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocha.Dialogs
{
    /// <summary> 
    /// Marks implementing class as a data for <see cref="IDialogModule"/>.
    /// </summary>
    public interface IDialog
    {
        /// <summary>
        /// Describes the result of dialog interaction.
        /// </summary>
        bool? DialogResult { get; set; }

        /// <summary>
        /// Value retrieved as a result of dialog interaction. 
        /// </summary>
        object DialogValue { get; set; }

        /// <summary>
        /// Standard parameters for dialog customization.
        /// </summary>
        DialogParameters DialogParameters { get; set; }

        /// <summary>
        /// Contains available actions for associated dialog.
        /// </summary>
        DialogActions DialogActions { get; set; }

        /// <summary>
        /// Contains a collection of events which can be subscribed be <see cref="IDialog"/> implementation.
        /// </summary>
        DialogEvents DialogEvents { get; set; }
    }
}
