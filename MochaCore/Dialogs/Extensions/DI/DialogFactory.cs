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
        public IDialogModule<T> Create<T>(string id) where T : new()
            => DialogManager.GetDialog<T>(id);

        /// <inheritdoc/>
        public IDataContextDialogModule<T> CreateDataContextModule<T>(string id) where T : new()
            => DialogManager.GetDataContextDialog<T>(id);

        /// <inheritdoc/>
        public ICustomDialogModule<T> CreateCustomModule<T>(string id) where T : new()
            => DialogManager.GetCustomDialog<T>(id);

        /// <inheritdoc/>
        public List<IDialogModule> GetOpenedDialogs(string id) => DialogManager.GetOpenedDialogs(id);

        /// <inheritdoc/>
        public List<IDialogModule> GetOpenedDialogs() => DialogManager.GetOpenedDialogs();
    }
}
