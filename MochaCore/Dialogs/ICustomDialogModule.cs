using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    public interface ICustomDialogModule : IDataContextDialogModule, IDialogClose, IDialogOpened, IDialogClosing
    {
        /// <inheritdoc/>
        IDataContextDialog? IDataContextDialogModule.DataContext => DataContext;

        public new ICustomDialog? DataContext { get; }

        /// <inheritdoc/>
        void IDataContextDialogModule.SetDataContext(IDataContextDialog? dataContext) => SetDataContext(dataContext);

        public void SetDataContext(ICustomDialog? dataContext);
    }

    /// <summary>
    /// Extends <see cref="IDialogModule{T}"/> with possibility to work with <see cref="ICustomDialog{T}"/> as DataContext.
    /// Provides <see cref="IDialogClose.Close"/> method, as well as <see cref="IDialogOpened.Opened"/> 
    /// and <see cref="IDialogClosing.Closing"/> events.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed properties for the associated dialog.</typeparam>
    public interface ICustomDialogModule<T> : ICustomDialogModule, IDataContextDialogModule<T> where T : new()
    {
        /// <summary>
        /// Returns a reference to <see cref="ICustomDialog{T}"/> object which acts as a DataContext for dialog represented by this module. 
        /// </summary>
        public new ICustomDialog<T>? DataContext { get; }

        /// <summary>
        /// Allows to assign new *DataContext* for this module.
        /// </summary>
        /// <param name="dataContext">DataContext to be assigned.</param>
        public void SetDataContext(ICustomDialog<T>? dataContext);

        /// <inheritdoc/>
        IDataContextDialog<T> IDataContextDialogModule<T>.DataContext => DataContext!;

        /// <inheritdoc/>
        void IDataContextDialogModule<T>.SetDataContext(IDataContextDialog<T>? dataContext) => SetDataContext(dataContext);

        ICustomDialog? ICustomDialogModule.DataContext => DataContext!;

        IDataContextDialog IDataContextDialogModule.DataContext => DataContext!;

    }
}
