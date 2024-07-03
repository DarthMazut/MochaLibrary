using System;
using System.ComponentModel;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// Marks implementing <see cref="IDialogModule"/> as able to provide <see cref="Closing"/> event.
    /// </summary>
    public interface IDialogClosing
    {
        // TODO: how can I obtain result invoked while closing?

        /// <summary>
        /// Fires when dialog is about to close.
        /// </summary>
        event EventHandler<CancelEventArgs>? Closing;
    }
}