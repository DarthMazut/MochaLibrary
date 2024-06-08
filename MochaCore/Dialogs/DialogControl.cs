using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    public class DialogControl : ICustomDialogControl
    {
        public ICustomDialogModule Module => throw new NotImplementedException();

        public object View => throw new NotImplementedException();

        public bool IsInitialized => throw new NotImplementedException();

        public event EventHandler Opened;
        public event EventHandler<CancelEventArgs> Closing;

        public void Close(bool? result)
        {
            throw new NotImplementedException();
        }
    }

    public class DialogControl<T> : DialogControl, ICustomDialogControl<T> where T : new()
    {
        IDataContextDialogModule IDataContextDialogControl.Module => throw new NotImplementedException();

        //IDataContextDialogModule<T> IDataContextDialogControl<T>.Module => throw new NotImplementedException();

        public new ICustomDialogModule<T> Module => throw new NotImplementedException();     
    }
}
