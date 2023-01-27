using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs.Extensions.DI
{
    /// <summary>
    /// Provides instances of dialogs registered by <see cref="DialogManager"/> class.
    /// </summary>
    public interface IDialogFactory
    {
        /// <summary>
        /// Returns new instance of <see cref="IDialogModule{T}"/> corresponding to given indetifier.
        /// </summary>
        /// <typeparam name="T">Type of properties object used by created dialog.</typeparam>
        /// <param name="id">Specifies the dialog identifier.</param>
        IDialogModule<T> Create<T>(string id) where T : DialogProperties, new();

        /// <summary>
        /// Returns new instacne of <see cref="IDataContextDialogModule{T}"/> corresponding to given identifier.
        /// </summary>
        /// <typeparam name="T">Type of properties object used by created dialog.</typeparam>
        /// <param name="id">Dialog identifier.</param>
        IDataContextDialogModule<T> CreateDataContextModule<T>(string id) where T : DialogProperties, new();

        /// <summary>
        /// Returns new instacne of <see cref="ICustomDialogModule{T}"/> corresponding to given identifier.
        /// </summary>
        /// <typeparam name="T">Type of properties object used by created dialog.</typeparam>
        /// <param name="id">Dialog identifier.</param>
        ICustomDialogModule<T> CreateCustomModule<T>(string id) where T : DialogProperties, new();

        /// <summary>
        /// Returns a collection of currently open <see cref="IDialogModule"/> with given ID.
        /// </summary>
        /// <param name="id">Specifies the dialog identifier.</param>
        List<IDialogModule> GetOpenedDialogs(string id);

        /// <summary>
        /// Returns a collection of all currently open <see cref="IDialogModule"/>.
        /// </summary>
        List<IDialogModule> GetOpenedDialogs();

    }
}
