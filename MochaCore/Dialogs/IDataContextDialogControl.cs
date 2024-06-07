using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    public interface IDataContextDialogControl
    {
        public bool IsInitialized { get; }

        public IDataContextDialogModule Module { get; }

        public object View { get; }
    }

    public interface IDataContextDialogControl<T> : IDataContextDialogControl where T : new()
    {
        public IDataContextDialogModule<T> Module { get; }
    }
}
