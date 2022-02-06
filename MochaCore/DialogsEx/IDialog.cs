using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    /// <summary>
    /// Base class for <see cref="IDialog{T}"/>.
    /// </summary>
    public interface IDialog
    {
        IDialogModule DialogModule { get; set; }
    }

    /// <summary> 
    /// Marks implementing class as dialog logic.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed parameters for the associated dialog.</typeparam>
    public interface IDialog<T>
    {
        /// <summary>
        /// Reference to <see cref="IDialogModule{T}"/> which this implementation is DataContext of.
        /// </summary>
        IDialogModule<T> DialogModule { get; set; }
    }
}
