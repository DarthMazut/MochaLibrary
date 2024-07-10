using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// Extends <see cref="IDataContextDialogModule"/> with <see cref="IDialogClose.Close(bool?)"/> method
    /// and <see cref="IDialogOpened.Opened"/> and <see cref="IDialogClosing.Closing"/> events.
    /// </summary>
    public interface ICustomDialogModule : IDataContextDialogModule, IDialogClose, IDialogOpened, IDialogClosing
    {

    }

    /// <summary>
    /// Extends <see cref="IDataContextDialogModule"/> with <see cref="IDialogClose.Close(bool?)"/> method
    /// and <see cref="IDialogOpened.Opened"/> and <see cref="IDialogClosing.Closing"/> events.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed properties for the associated dialog.</typeparam>
    public interface ICustomDialogModule<T> : ICustomDialogModule, IDataContextDialogModule<T> where T : new()
    {

    }
}
