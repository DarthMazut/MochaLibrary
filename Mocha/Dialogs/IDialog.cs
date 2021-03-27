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
    public interface IDialog<T>
    {
        /// <summary>
        /// Exposes API for dialog interaction.
        /// </summary>
        DialogControl<T> DialogControl { get; }
    }

    /// <summary> 
    /// Marks implementing class as a data for <see cref="IDialogModule"/>.
    /// </summary>
    public interface IDialog
    {
        /// <summary>
        /// Exposes API for dialog interaction.
        /// </summary>
        DialogControl DialogControl { get; }
    }
}
