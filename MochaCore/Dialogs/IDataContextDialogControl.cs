using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    public interface IDataContextDialogControl
    {
        public object View { get; }

        public IDataContextDialogModule Module { get; }

        public bool IsInitialized { get; }
    }

    public interface IDataContextDialogControl<T> : IDataContextDialogControl where T : new()
    {
        public new IDataContextDialogModule<T> Module { get; }
    }
}
