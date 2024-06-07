using System;

namespace MochaCore.Dialogs
{
    public interface ICustomDialog : IDataContextDialog
    {
        /// <inheritdoc/>
        IDataContextDialogControl IDataContextDialog.DialogControl => DialogControl;

        public new ICustomDialogControl DialogControl { get; }
    }

    /// <summary> 
    /// Marks implementing class as dialog logic (DataContext) for 
    /// <see cref="ICustomDialogModule{T}"/> implementations.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed parameters for the associated dialog.</typeparam>
    public interface ICustomDialog<T> : ICustomDialog, IDataContextDialog<T> where T : new()
    {
        /// <summary>
        /// Exposes API for interaction with <see cref="ICustomDialogModule{T}"/>.
        /// </summary>
        public new ICustomDialogControl<T> DialogControl { get; }

        /// <inheritdoc/>
        IDataContextDialogControl<T> IDataContextDialog<T>.DialogControl => DialogControl;
    }
}
