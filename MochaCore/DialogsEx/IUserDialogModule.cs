using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    /// <summary>
    /// Extends <see cref="ICustomDialogModule{T}"/> with <see cref="IDialogOpened.Opened"/> and <see cref="IDialogClosing.Closing"/> events.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed properties for the associated dialog.</typeparam>
    public interface IUserDialogModule<T> : IDialogOpened, IDialogClosing, ICustomDialogModule<T>
    {

    }
}
