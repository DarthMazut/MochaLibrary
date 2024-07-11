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
        /// Retrieves a new instance of the <see cref="IDialogModule"/> implementation corresponding to the provided identifier.
        /// </summary>
        /// <param name="id">The identifier of the dialog module to retrieve.</param>
        IDialogModule RetrieveDialog(string id);

        /// <summary>
        /// Retrieves a new instance of the <see cref="IDialogModule{T}"/> implementation corresponding to the provided identifier.
        /// </summary>
        /// <typeparam name="T">Type of statycially typed properties object used for customization of retrieved module.</typeparam>
        /// <param name="id">The identifier of the dialog module to retrieve.</param>
        IDialogModule<T> RetrieveDialog<T>(string id) where T : new();

        /// <summary>
        /// Retrieves a new instance of the <see cref="IDataContextDialogModule"/> implementation corresponding to the provided identifier.
        /// </summary>
        /// <param name="id">The identifier of the dialog module to retrieve.</param>
        IDataContextDialogModule RetrieveDataContextDialog(string id);

        /// <summary>
        /// Retrieves a new instance of the <see cref="IDataContextDialogModule{T}"/> implementation corresponding to the provided identifier.
        /// </summary>
        /// <typeparam name="T">Type of statycially typed properties object used for customization of retrieved module.</typeparam>
        /// <param name="id">The identifier of the dialog module to retrieve.</param>
        IDataContextDialogModule<T> RetrieveDataContextDialog<T>(string id) where T : new();

        /// <summary>
        /// Retrieves a new instance of the <see cref="ICustomDialogModule"/> implementation corresponding to the provided identifier.
        /// </summary>
        /// <param name="id">The identifier of the dialog module to retrieve.</param>
        ICustomDialogModule RetrieveCustomDialog(string id);

        /// <summary>
        /// Retrieves a new instance of the <see cref="ICustomDialogModule{T}"/> implementation corresponding to the provided identifier.
        /// </summary>
        /// <typeparam name="T">Type of statycially typed properties object used for customization of retrieved module.</typeparam>
        /// <param name="id">The identifier of the dialog module to retrieve.</param>
        ICustomDialogModule<T> RetrieveCustomDialog<T>(string id) where T : new();

        /// <summary>
        /// Retrieves a read-only collection of all <see cref="IDialogModule"/> objects that have been opened but not yet disposed.
        /// </summary>
        IReadOnlyCollection<IDialogModule> GetOpenedDialogs();

        /// <summary>
        /// Retrieves a read-only collection of all <see cref="IDialogModule"/> objects with provided ID that have been opened but not yet disposed.
        /// </summary>
        /// <param name="id">The ID of the modules to be included.</param>
        IReadOnlyCollection<IDialogModule> GetOpenedDialogs(string id);
    }
}
