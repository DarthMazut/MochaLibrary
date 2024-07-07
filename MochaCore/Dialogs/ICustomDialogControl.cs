using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// Provides abstraction for <see cref="CustomDialogControl"/>.
    /// </summary>
    public interface ICustomDialogControl : IDataContextDialogControl, IDialogClose, IDialogOpened, IDialogClosing
    {
        /// <inheritdoc/>
        IDataContextDialogModule IDataContextDialogControl.Module => Module;

        /// <summary>
        /// Returns the related <see cref="ICustomDialogModule"/> object.
        /// </summary>
        public new ICustomDialogModule Module { get; }
    }

    /// <summary>
    /// Provides abstraction for <see cref="CustomDialogControl{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the dialog properties object associated with related dialog module.</typeparam>
    public interface ICustomDialogControl<T> : ICustomDialogControl, IDataContextDialogControl<T> where T : new()
    {
        /// <inheritdoc/>
        IDataContextDialogModule<T> IDataContextDialogControl<T>.Module => Module;

        /// <summary>
        /// Returns the related <see cref="ICustomDialogModule{T}"/> object.
        /// </summary>
        public new ICustomDialogModule<T> Module { get; }
    }
}
