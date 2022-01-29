using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    /// <summary>
    /// Extends <see cref="IDialogModule{T}"/> with possibility to work with <see cref="IDialog"/> as DataContext.
    /// Allows for displaying represented dialog in non-modal manner and closing it with <c>Close()</c> method.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed properties for the associated dialog.</typeparam>
    public interface ICustomDialogModule<T> : IDialogModule<T>
    {
        /// <summary>
        /// Returns a reference to <see cref="IDialog"/> object which acts as a DataContext for dialog represented by this module. 
        /// </summary>
        IDialog<T> DataContext { get; }

        /// <summary>
        /// Allows to assign new *DataContext* for this module.
        /// </summary>
        /// <param name="dataContext">DataContext to be assigned.</param>
        void SetDataContext(IDialog<T> dataContext);

        /// <summary>
        /// Asynchronously opens a dialog represented by this instance in non-modal mode. 
        /// </summary>
        /// <param name="host">
        /// The object which hosts this dialog while being shown.
        /// <para>You should pass '<see langword="this"/>' here.</para>
        /// </param>
        Task ShowAsync(object host);

        /// <summary>
        /// Closes the dialog if open.
        /// </summary>
        void Close();

    }
}
