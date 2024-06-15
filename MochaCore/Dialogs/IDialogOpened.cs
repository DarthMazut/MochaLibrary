using System;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// Marks implementing <see cref="IDialogModule"/> as able to provide <see cref="Opened"/> event.
    /// </summary>
    public interface IDialogOpened
    {
        /// <summary>
        /// Fires right after the dialog opens.
        /// </summary>
        event EventHandler? Opened;
    }
}