using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    public interface IDialog
    {
        IDialogModule DialogModule { get; set; }
    }

    /// <summary> 
    /// Marks implementing class as dialog logic.
    /// </summary>
    public interface IDialog<T>
    {
        IDialogModule<T> DialogModule { get; set; }
    }
}
