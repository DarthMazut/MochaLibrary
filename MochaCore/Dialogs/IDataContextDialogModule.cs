using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// Extends <see cref="IDialogModule{T}"/> with possibility to work with <see cref="IDataContextDialog"/> as DataContext.
    /// </summary>
    public interface IDataContextDialogModule : IDialogModule
    {
        /// <summary>
        /// Returns a reference to <see cref="IDataContextDialog"/> object which acts as a DataContext for dialog represented by this module.
        /// </summary>
        public IDataContextDialog? DataContext { get; }

        /// <summary>
        /// Allows to assign new *DataContext* for this module.
        /// </summary>
        /// <param name="dataContext">DataContext to be assigned.</param>
        public void SetDataContext(IDataContextDialog? dataContext);
    }

    /// <summary>
    /// Extends <see cref="IDialogModule{T}"/> with possibility to work with <see cref="IDataContextDialog{T}"/> as DataContext.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed properties for the associated dialog.</typeparam>
    public interface IDataContextDialogModule<T> : IDataContextDialogModule, IDialogModule<T> where T : new()
    {
        
    }
}
