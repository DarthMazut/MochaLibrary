using System.Collections.Generic;

namespace MochaCore.Dialogs.Extensions.DI
{
    /// <summary>
    /// Provides an implementation of <see cref="IDialogFactory"/> interface.
    /// </summary>
    public class DialogFactory : IDialogFactory
    {
        /// <inheritdoc/>
        public IDialogModule Create(string id)
        {
            return DialogManager.GetDialog(id);
        }

        /// <inheritdoc/>
        public IDialogModule<T> Create<T>(string id) where T : DialogControl
        {
            return (IDialogModule<T>)DialogManager.GetDialog(id);
        }

        /// <inheritdoc/>
        public List<IDialogModule> GetActiveDialogs(string id)
        {
            return DialogManager.GetActiveDialogs(id);
        }

        /// <inheritdoc/>
        public List<IDialogModule> GetActiveDialogs()
        {
            return DialogManager.GetActiveDialogs();
        }

    }
}
