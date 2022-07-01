using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    /// <summary>
    /// Extends <see cref="IDialogModule{T}"/> with possibility to work with <see cref="IDataContextDialog{T}"/> as DataContext.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed properties for the associated dialog.</typeparam>
    public interface IDataContextDialogModule<T> : IDialogModule<T> where T : DialogProperties, new()
    {
        /// <summary>
        /// Returns a reference to <see cref="IDialog"/> object which acts as a DataContext for dialog represented by this module. 
        /// </summary>
        IDataContextDialog<T>? DataContext { get; }

        /// <summary>
        /// Allows to assign new *DataContext* for this module.
        /// </summary>
        /// <param name="dataContext">DataContext to be assigned.</param>
        void SetDataContext(IDataContextDialog<T>? dataContext);
    }
}
