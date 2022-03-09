using MochaCore.DialogsEx.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    /// <summary>
    /// Extends <see cref="IDialogModule{T}"/> with possibility to work with <see cref="IDataContextDialog{T}"/> as DataContext.
    /// Provides <see cref="IDialogClose.Close"/> method.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed properties for the associated dialog.</typeparam>
    public interface IDataContextDialogModule<T> : IDialogModule<T>, IDialogDataContext<IDataContextDialog<T>>, IDialogClose
    {

    }
}
