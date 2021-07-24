using System.Collections.Generic;

namespace MochaCore.Dialogs.Extensions.DI
{
    /// <summary>
    /// Provides instances of dialogs registered by <see cref="DialogManager"/> class.
    /// </summary>
    public interface IDialogFactory
    {
        /// <summary>
        /// Returns new instance of <see cref="IDialogModule"/> corresponding to given identifier.
        /// </summary>
        /// <param name="id">Specifies the dialog identifier.</param>
        IDialogModule Create(string id);

        /// <summary>
        /// Returns new instance of <see cref="IDialogModule{T}"/> corresponding to given indetifier.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="DialogControl"/> object used by created dialog.</typeparam>
        /// <param name="id">Specifies the dialog identifier.</param>
        IDialogModule<T> Create<T>(string id) where T : DialogControl;

        /// <summary>
        /// Returns a collection of instantiated <see cref="IDialogModule"/> with given ID which haven't been disposed yet.
        /// </summary>
        /// <param name="id">Specifies the dialog identifier.</param>
        List<IDialogModule> GetActiveDialogs(string id);

        /// <summary>
        /// Returns a collection of all instantiated <see cref="IDialogModule"/>, which haven't been disposed yet.
        /// </summary>
        List<IDialogModule> GetActiveDialogs();
    }
}
