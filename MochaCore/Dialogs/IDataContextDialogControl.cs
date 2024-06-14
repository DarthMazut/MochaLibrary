using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public event EventHandler? Closed;

        public event EventHandler? Disposed;

        public event EventHandler? Opening;

        public event EventHandler? Initialized;

        public bool TryClose(bool? result);

        public IDisposable TrySubscribeDialogOpened(EventHandler openedHandler);

        public IDisposable TrySubscribeDialogOpened(EventHandler openedHandler, Action<IDataContextDialogModule> featureUnavailableHandler);

        public IDisposable TrySubscribeDialogClosing(EventHandler<CancelEventArgs> closingHandler);

        public IDisposable TrySubscriveDialogClosing(EventHandler<CancelEventArgs> closingHandler, Action<IDataContextDialogModule> featureUnavailableHandler);
    }

    public interface IDataContextDialogControl<T> : IDataContextDialogControl where T : new()
    {
        public new IDataContextDialogModule<T> Module { get; }

        public T Properties { get; set; }
    }
}
