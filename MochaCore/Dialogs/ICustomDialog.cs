﻿using System;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// Marks implementing class as data context for <see cref="ICustomDialogModule"/>.
    /// </summary>
    public interface ICustomDialog : IDataContextDialog
    {
        /// <inheritdoc/>
        IDataContextDialogControl IDataContextDialog.DialogControl => DialogControl;

        public new ICustomDialogControl DialogControl { get; }
    }

    /// <summary> 
    /// Marks implementing class as data context for <see cref="ICustomDialogModule{T}"/>.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed parameters for the associated dialog.</typeparam>
    public interface ICustomDialog<T> : ICustomDialog, IDataContextDialog<T> where T : new()
    {
        /// <summary>
        /// Exposes API for interaction with <see cref="ICustomDialogModule{T}"/>.
        /// </summary>
        public new ICustomDialogControl<T> DialogControl { get; }

        /// <inheritdoc/>
        IDataContextDialogControl IDataContextDialog.DialogControl => DialogControl;

        /// <inheritdoc/>
        IDataContextDialogControl<T> IDataContextDialog<T>.DialogControl => DialogControl;

        /// <inheritdoc/>
        ICustomDialogControl ICustomDialog.DialogControl => DialogControl; 
    }
}
