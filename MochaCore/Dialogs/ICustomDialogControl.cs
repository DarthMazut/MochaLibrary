using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    public interface ICustomDialogControl : IDataContextDialogControl, IDialogClose, IDialogOpened, IDialogClosing
    {
        /// <inheritdoc/>
        IDataContextDialogModule IDataContextDialogControl.Module => Module;

        public new ICustomDialogModule Module { get; }
    }

    public interface ICustomDialogControl<T> : ICustomDialogControl, IDataContextDialogControl<T> where T : new()
    {
        IDataContextDialogModule<T> IDataContextDialogControl<T>.Module => Module;

        //ICustomDialogModule ICustomDialogControl.Module => Module;

        public new ICustomDialogModule<T> Module { get; }
    }
}
