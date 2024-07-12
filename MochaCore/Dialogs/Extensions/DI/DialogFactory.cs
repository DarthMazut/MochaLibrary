using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs.Extensions.DI
{
    /// <summary>
    /// Provides implementation of <see cref="IDialogFactory"/>.
    /// </summary>
    public class DialogFactory : IDialogFactory
    {
        /// <inheritdoc/>
        public ICustomDialogModule RetrieveCustomDialog(string id)
            => DialogManager.RetrieveCustomDialog(id);

        /// <inheritdoc/>
        public ICustomDialogModule<T> RetrieveCustomDialog<T>(string id) where T : new()
            => DialogManager.RetrieveCustomDialog<T>(id);

        /// <inheritdoc/>
        public IDataContextDialogModule RetrieveDataContextDialog(string id)
            => DialogManager.RetrieveDataContextDialog(id);

        /// <inheritdoc/>
        public IDataContextDialogModule<T> RetrieveDataContextDialog<T>(string id) where T : new()
             => DialogManager.RetrieveDataContextDialog<T>(id);

        /// <inheritdoc/>
        public IDialogModule RetrieveDialog(string id)
            => DialogManager.RetrieveDialog(id);

        /// <inheritdoc/>
        public IDialogModule<T> RetrieveDialog<T>(string id) where T : new()
            => DialogManager.RetrieveDialog<T>(id);

        /// <inheritdoc/>
        public IReadOnlyCollection<IDialogModule> GetOpenedDialogs()
            => DialogManager.GetOpenedDialogs();
        
        /// <inheritdoc/>
        public IReadOnlyCollection<IDialogModule> GetOpenedDialogs(string id)
            => DialogManager.GetOpenedDialogs(id);
    }
}
