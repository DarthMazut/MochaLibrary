using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    /// <summary>
    /// Extends <see cref="IDialogModule{T}"/> with possibility to work with <see cref="IDialog"/> as DataContext.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed properties for the associated dialog.</typeparam>
    public interface ICustomDialogModule<T> : IDialogModule<T>
    {
        /// <summary>
        /// Returns a reference to <see cref="IDialog"/> object which acts as a DataContext for dialog represented by this module. 
        /// </summary>
        IDialog<T>? DataContext { get; }

        /// <summary>
        /// Allows to assign new *DataContext* for this module.
        /// </summary>
        /// <param name="dataContext">DataContext to be assigned.</param>
        void SetDataContext(IDialog<T> dataContext);
    }
}
