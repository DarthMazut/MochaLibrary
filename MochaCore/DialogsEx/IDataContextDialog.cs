﻿namespace MochaCore.DialogsEx
{
    /// <summary> 
    /// Marks implementing class as dialog logic (DataContext) for 
    /// <see cref="IDataContextDialogModule{T}"/> implementations.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed parameters for the associated dialog.</typeparam>
    public interface IDataContextDialog<T>
    {
        /// <summary>
        /// A reference to the module whose DataContext is this instance. 
        /// </summary>
        IDataContextDialogModule<T> DialogModule { get; set; }
    }
}