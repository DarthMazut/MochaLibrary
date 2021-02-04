using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocha.Dialogs
{
    /// <summary>
    /// Allows implementing object to be treated as a *DataContext* or backend data for <see cref="IDialogModule"/>. 
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
    }
}
