using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs.Extensions.DI
{
    public class DialogFactory : IDialogFactory
    {
        /// <inheritdoc/>
        public IDialogModule<T> Create<T>(string id) where T : DialogProperties, new()
        {
            return DialogManager.GetDialog<T>(id);
        }

        /// <inheritdoc/>
        public IDataContextDialogModule<T> CreateDataContextModule<T>(string id) where T : DialogProperties, new()
        {
            return DialogManager.GetDataContextDialog<T>(id);
        }

        /// <inheritdoc/>
        public ICustomDialogModule<T> CreateCustomModule<T>(string id) where T : DialogProperties, new()
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
