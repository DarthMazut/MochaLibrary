using System;

namespace MochaCore.Dialogs
{
    /// <summary> 
    /// Marks implementing class as dialog logic (DataContext) for 
    /// <see cref="ICustomDialogModule{T}"/> implementations.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed parameters for the associated dialog.</typeparam>
    public interface ICustomDialog<T> : IDataContextDialog<T> where T : DialogProperties, new()
    {
        /// <summary>
        /// Exposes API for interaction with <see cref="ICustomDialogModule{T}"/>.
        /// </summary>
        new CustomDialogControl<T> DialogControl { get; }

        DataContextDialogControl<T> IDataContextDialog<T>.DialogControl => DialogControl;
    }
}
