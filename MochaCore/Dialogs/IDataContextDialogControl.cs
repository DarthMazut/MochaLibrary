using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// Provides abstraction for <see cref="DataContextDialogControl"/>.
    /// </summary>
    public interface IDataContextDialogControl
    {
        /// <summary>
        /// Technology-specific dialog object which is represented by related <see cref="IDataContextDialogModule"/>.
        /// </summary>
        public object View { get; }

        /// <summary>
        /// Returns the related <see cref="IDataContextDialogModule"/> object.
        /// </summary>
        public IDataContextDialogModule Module { get; }

        /// <summary>
        /// Determines whether the current instance has been initialized and is ready for full interaction.
        /// </summary>
        public bool IsInitialized { get; }

        /// <summary>
        /// Occurs when related dialog closes.
        /// </summary>
        public event EventHandler? Closed;

        /// <summary>
        /// Occurs when related module is disposed.
        /// </summary>
        public event EventHandler? Disposed;

        /// <summary>
        /// Fires whenever dialog is about to be opened.
        /// </summary>
        public event EventHandler? Opening;

        /// <summary>
        /// Occurs when current instance gets initialized.
        /// </summary>
        public event EventHandler? Initialized;

        /// <summary>
        /// Attempts to close the related <see cref="IDataContextDialogModule"/> if it is currently open. 
        /// Returns <see langword="true"/> if the dialog supports closing (regardless of whether the dialog was
        /// actually closed by calling this method), or <see langword="false"/> if it does not support closing.
        /// Throws an <see cref="InvalidOperationException"/> if DialogControl hasn't been initialized at the time this
        /// method was invoked.
        /// </summary>
        /// <param name="result">Determines the result of dialog interaction.</param>
        public bool TryClose(bool? result);

        /// <summary>
        /// Equeues request to subscribe to <see cref="IDialogOpened.Opened"/> of related module during initialization 
        /// of this instance. If related module implements <see cref="IDialogOpened.Opened"/>, subscription will occur 
        /// at initialization time.
        /// </summary>
        /// <param name="openedHandler">A delegate to be called when <see cref="IDialogOpened.Opened"/> event occures.</param>
        /// <returns>Object representing enqueued request. Dispose it to cancel subscription.</returns>
        public IDisposable TrySubscribeDialogOpened(EventHandler openedHandler);

        /// <summary>
        /// Equeues request to subscribe to <see cref="IDialogOpened.Opened"/> of related module during initialization 
        /// of this instance. If related module implements <see cref="IDialogOpened.Opened"/>, subscription will occur 
        /// otherwise provided unavailable handler will be called at initialization time.
        /// </summary>
        /// <param name="openedHandler">A delegate to be called when <see cref="IDialogOpened.Opened"/> event occures.</param>
        /// <param name="featureUnavailableHandler">A delegate to be called when related module does not provide <see cref="IDialogOpened.Opened"/> event.</param>
        /// <returns>Object representing enqueued request. Dispose it to cancel subscription or unavailable handler.</returns>
        public IDisposable TrySubscribeDialogOpened(EventHandler openedHandler, Action<IDataContextDialogModule>? featureUnavailableHandler);

        /// <summary>
        /// Equeues request to subscribe to <see cref="IDialogClosing.Closing"/> of related module during initialization 
        /// of this instance. If related module implements <see cref="IDialogClosing.Closing"/>, subscription will occur 
        /// at initialization time.
        /// </summary>
        /// <param name="closingHandler">A delegate to be called when <see cref="IDialogClosing.Closing"/> event occures.</param>
        /// <returns>Object representing enqueued request. Dispose it to cancel subscription.</returns>
        public IDisposable TrySubscribeDialogClosing(EventHandler<CancelEventArgs> closingHandler);

        /// <summary>
        /// Equeues request to subscribe to <see cref="IDialogClosing.Closing"/> of related module during initialization 
        /// of this instance. If related module implements <see cref="IDialogClosing.Closing"/>, subscription will occur 
        /// otherwise provided unavailable handler will be called at initialization time.
        /// </summary>
        /// <param name="closingHandler">A delegate to be called when <see cref="IDialogClosing.Closing"/> event occures.</param>
        /// <param name="featureUnavailableHandler">A delegate to be called when related module does not provide <see cref="IDialogClosing.Closing"/> event.</param>
        /// <returns>Object representing enqueued request. Dispose it to cancel subscription or unavailable handler.</returns>
        public IDisposable TrySubscribeDialogClosing(EventHandler<CancelEventArgs> closingHandler, Action<IDataContextDialogModule>? featureUnavailableHandler);
    }

    /// <summary>
    /// Provides abstraction for <see cref="DataContextDialogControl{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the dialog properties object associated with related dialog module.</typeparam>
    public interface IDataContextDialogControl<T> : IDataContextDialogControl where T : new()
    {
        /// <summary>
        /// Returns the related <see cref="IDataContextDialogModule{T}"/> object.
        /// </summary>
        public new IDataContextDialogModule<T> Module { get; }

        /// <summary>
        /// Returns statically typed properties which allows for customization of related 
        /// <see cref="IDataContextDialogModule{T}"/> object.
        /// </summary>
        public T Properties { get; }

        /// <summary>
        /// Enqueue customization delegate to be called during initialization time.
        /// </summary>
        /// <param name="customizeDelegate">Customization delegate.</param>
        public void Customize(Action<T> customizeDelegate);
    }
}
