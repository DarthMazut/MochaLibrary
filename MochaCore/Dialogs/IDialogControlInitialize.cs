namespace MochaCore.Dialogs
{
    /// <summary>
    /// Provides a method to initialize an <see cref="IDataContextDialogControl"/> object.
    /// </summary>
    public interface IDialogControlInitialize
    {
        /// <summary>
        /// Initializes an <see cref="IDataContextDialogControl"/> object based on the provided module.
        /// </summary>
        /// <param name="module">The module required for initialization.</param>
        public void Initialize(IDataContextDialogModule module);
    }
}