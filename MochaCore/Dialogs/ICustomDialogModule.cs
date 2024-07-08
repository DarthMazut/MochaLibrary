using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomDialogModule : IDataContextDialogModule, IDialogClose, IDialogOpened, IDialogClosing
    {

    }

    /// <summary>
    /// Extends <see cref="IDialogModule{T}"/> with possibility to work with <see cref="ICustomDialog{T}"/> as DataContext.
    /// Provides <see cref="IDialogClose.Close"/> method, as well as <see cref="IDialogOpened.Opened"/> 
    /// and <see cref="IDialogClosing.Closing"/> events.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed properties for the associated dialog.</typeparam>
    public interface ICustomDialogModule<T> : ICustomDialogModule, IDataContextDialogModule<T> where T : new()
    {

    }
}
