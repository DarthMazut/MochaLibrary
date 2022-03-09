using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    /// <summary>
    /// Provides API to work with DataContexts for <see cref="IDialogModule{T}"/> descendants.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed properties for the associated dialog.</typeparam>
    public interface IDialogDataContext<T> where T : IDialog
    {
        /// <summary>
        /// Returns a reference to <see cref="IDialog"/> object which acts as a DataContext for dialog represented by this module. 
        /// </summary>
        T DataContext { get; }

        /// <summary>
        /// Allows to assign new *DataContext* for this module.
        /// </summary>
        /// <param name="dataContext">DataContext to be assigned.</param>
        void SetDataContext(T dataContext);
    }
}
