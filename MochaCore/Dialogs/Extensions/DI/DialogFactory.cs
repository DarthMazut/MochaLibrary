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
        public ICustomDialogModule RetrieveCustomDialog(string id)
        {
            throw new NotImplementedException();
        }

        public ICustomDialogModule<T> RetrieveCustomDialog<T>(string id) where T : new()
        {
            throw new NotImplementedException();
        }

        public IDataContextDialogModule RetrieveDataContextDialog(string id)
        {
            throw new NotImplementedException();
        }

        public IDataContextDialogModule<T> RetrieveDataContextDialog<T>(string id) where T : new()
        {
            throw new NotImplementedException();
        }

        public IDialogModule RetrieveDialog(string id)
        {
            throw new NotImplementedException();
        }

        public IDialogModule<T> RetrieveDialog<T>(string id) where T : new()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<IDialogModule> GetOpenedDialogs()
            => DialogManager.GetOpenedDialogs();

        public IReadOnlyCollection<IDialogModule> GetOpenedDialogs(string id)
            => DialogManager.GetOpenedDialogs(id);
    }
}
