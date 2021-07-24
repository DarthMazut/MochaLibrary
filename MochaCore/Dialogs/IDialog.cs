namespace MochaCore.Dialogs
{
    /// <summary> 
    /// Marks implementing class as dialog logic.
    /// </summary>
    public interface IDialog
    {
        /// <summary>
        /// Exposes API for dialog management.
        /// </summary>
        DialogControl DialogControl { get; }
    }

    /// <summary>
    /// Marks implementing class as dialog logic.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed parameters for the associated dialog.</typeparam>
    public interface IDialog<T> : IDialog where T : DialogControl
    {
        /// <summary>
        /// Exposes API for dialog management.
        /// </summary>
        new T DialogControl { get; }
    }
}
