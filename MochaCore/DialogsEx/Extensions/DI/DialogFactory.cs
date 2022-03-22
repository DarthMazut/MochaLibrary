using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx.Extensions.DI
{
    public class DialogFactory : IDialogFactory
    {
        /// <inheritdoc/>
        public IDialogModule<T> Create<T>(string id)
        {
            return DialogManager.GetDialog<T>(id);
        }

        /// <inheritdoc/>
        public IDataContextDialogModule<T> CreateDataContextModule<T>(string id)
        {
            return DialogManager.GetDataContextDialog<T>(id);
        }

        /// <inheritdoc/>
        public ICustomDialogModule<T> CreateCustomModule<T>(string id)
        {
            return DialogManager.GetCustomDialog<T>(id);
        }

        /// <inheritdoc/>
        public List<IDialogModule> GetOpenedDialogs(string id)
        {
            return DialogManager.GetOpenedDialogs(id);
        }

        /// <inheritdoc/>
        public List<IDialogModule> GetOpenedDialogs()
        {
            return DialogManager.GetOpenedDialogs();
        }
    }
}
