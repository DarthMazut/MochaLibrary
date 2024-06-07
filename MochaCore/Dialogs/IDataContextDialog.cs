namespace MochaCore.Dialogs
{
    /// <summary> 
    /// Marks implementing class as dialog logic (DataContext) for 
    /// <see cref="IDataContextDialogModule"/> implementations.
    /// </summary>
    public interface IDataContextDialog
    {
        /// <summary>
        /// Exposes API for dialog interaction.
        /// </summary>
        public IDataContextDialogControl DialogControl { get; }
    }

    /// <summary> 
    /// Marks implementing class as dialog logic (DataContext) for 
    /// <see cref="IDataContextDialogModule{T}"/> implementations.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed parameters for the associated dialog.</typeparam>
    public interface IDataContextDialog<T> : IDataContextDialog where T : new()
    {
        /// <inheritdoc/>
        IDataContextDialogControl IDataContextDialog.DialogControl => DialogControl;

        /// <summary>
        /// Exposes API for dialog interaction.
        /// </summary>
        public new IDataContextDialogControl<T> DialogControl { get; }
    }
}
