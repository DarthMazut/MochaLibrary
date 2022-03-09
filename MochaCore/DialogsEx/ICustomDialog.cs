namespace MochaCore.DialogsEx
{
    /// <summary> 
    /// Marks implementing class as dialog logic (DataContext) for 
    /// <see cref="ICustomDialogModule{T}"/> implementations.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed parameters for the associated dialog.</typeparam>
    public interface ICustomDialog<T> : IDialog
    {
        /// <summary>
        /// A reference to the module whose DataContext is this instance. 
        /// </summary>
        ICustomDialogModule<T> DialogModule { get; set; }
    }
}
