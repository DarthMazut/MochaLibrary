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

        // Standard parameters for dialog customization.

        /// <summary>
        /// Allows for customization of associated dialog.
        /// Use <see cref="DialogParameters"/> to obtain standard keys.
        /// </summary>
        Dictionary<string, object> Parameters { get; set; }
    }
}
