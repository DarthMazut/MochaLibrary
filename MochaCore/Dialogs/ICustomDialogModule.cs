using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// Extends <see cref="IDialogModule{T}"/> with possibility to work with <see cref="ICustomDialog{T}"/> as DataContext.
    /// Provides <see cref="IDialogClose.Close"/> method, as well as <see cref="IDialogOpened.Opened"/> 
    /// and <see cref="IDialogClosing.Closing"/> events.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed properties for the associated dialog.</typeparam>
    public interface ICustomDialogModule<T> : IDataContextDialogModule<T>, IDialogClose, IDialogOpened, IDialogClosing where T : DialogProperties, new()
    {
        /// <summary>
        /// Returns a reference to <see cref="IDialog"/> object which acts as a DataContext for dialog represented by this module. 
        /// </summary>
        new ICustomDialog<T>? DataContext { get; }

        /// <summary>
        /// Allows to assign new *DataContext* for this module.
        /// </summary>
        /// <param name="dataContext">DataContext to be assigned.</param>
        void SetDataContext(ICustomDialog<T>? dataContext);

        IDataContextDialog<T> IDataContextDialogModule<T>.DataContext => DataContext!;

        void IDataContextDialogModule<T>.SetDataContext(IDataContextDialog<T>? dataContext)
        {
            SetDataContext(dataContext);
        }
    }

}
