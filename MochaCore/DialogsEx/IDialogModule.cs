using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    public interface IDialogModule
    {
        object? View { get; }

        object? Parent { get; }

        Task<bool?> ShowModalAsync(object host);

        event EventHandler Opening;

        event EventHandler Closed;

        event EventHandler Disposed;
    }

    public interface IDialogModule<T> : IDialogModule where T : DialogProperties
    {
        T Properties { get; set; }
    }
}
