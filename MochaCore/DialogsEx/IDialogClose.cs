using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    /// <summary>
    /// Marks implementing <see cref="IDialogModule"/> as able to perform <see cref="Close"/> operation.
    /// </summary>
    public interface IDialogClose
    {
        /// <summary>
        /// Closes the dialog if open.
        /// </summary>
        void Close();
    }
}
