using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Provides API form managing related window.
    /// </summary>
    public interface IBaseWindowControl
    {
        /// <summary>
        /// Returns the technolog-specific window object.
        /// </summary>
        public object View { get; }

        /// <summary>
        /// Returns related <see cref="IBaseWindowModule"/> instance.
        /// </summary>
        public IBaseWindowModule Module { get;}

        /// <summary>
        /// Indicates whether current instance is initialized.
        /// </summary>
        public bool IsInitialized { get; }

        /// <summary>
        /// Occurs when related window is opened.
        /// </summary>
        public event EventHandler? Opened;

        /// <summary>
        /// Occurs when related window closes.
        /// </summary>
        public event EventHandler? Closed;

        /// <summary>
        /// Occurs when related module is disposed.
        /// </summary>
        public event EventHandler? Disposed;

        /// <summary>
        /// Equeues request to subscribe to <see cref="IClosingWindow"/> of related module during initialization 
        /// of this instance. If related module implements <see cref="IClosingWindow"/>, subscription will occur
        /// at initialization time.
        /// </summary>
        /// <param name="closingHandler">A delegate to be called when <see cref="IClosingWindow.Closing"/> event occures.</param>
        /// <returns>Object representing enqueued request. Dispose it to cancel subscription or unavailable handler.</returns>
        public IDisposable TrySubscribeWindowClosing(EventHandler<CancelEventArgs> closingHandler);

        /// <summary>
        /// Equeues request to subscribe to <see cref="IClosingWindow"/> of related module during initialization 
        /// of this instance. If related module implements <see cref="IClosingWindow"/>, subscription will occur 
        /// otherwise provided unavailable handler will be called at initialization time.
        /// </summary>
        /// <param name="closingHandler">A delegate to be called when <see cref="IClosingWindow.Closing"/> event occures.</param>
        /// <param name="featureUnavailableHandler">A delegate to be called when related module does not provide <see cref="IClosingWindow.Closing"/> event.</param>
        /// <returns>Object representing enqueued request. Dispose it to cancel subscription or unavailable handler.</returns>
        public IDisposable TrySubscribeWindowClosing(EventHandler<CancelEventArgs> closingHandler, Action<IBaseWindowModule>? featureUnavailableHandler);

        /// <summary>
        /// Equeues request to subscribe to <see cref="IWindowStateChanged"/> of related module during initialization 
        /// of this instance. If related module implements <see cref="IWindowStateChanged"/>, subscription will occur
        /// at initialization time.
        /// </summary>
        /// <param name="stateChangedHandler">A delegate to be called when <see cref="IWindowStateChanged.StateChanged"/> event occures.</param>
        /// <returns>Object representing enqueued request. Dispose it to cancel subscription or unavailable handler.</returns>
        public IDisposable TrySubscribeWindowStateChanged(EventHandler<WindowStateChangedEventArgs> stateChangedHandler);

        /// <summary>
        /// Equeues request to subscribe to <see cref="IWindowStateChanged"/> of related module during initialization 
        /// of this instance. If related module implements <see cref="IWindowStateChanged"/>, subscription will occur 
        /// otherwise provided unavailable handler will be called at initialization time.
        /// </summary>
        /// <param name="stateChangedHandler">A delegate to be called when <see cref="IWindowStateChanged.StateChanged"/> event occures.</param>
        /// <param name="featureUnavailableHandler">A delegate to be called when related module does not provide <see cref="IWindowStateChanged.StateChanged"/> event.</param>
        /// <returns>Object representing enqueued request. Dispose it to cancel subscription or unavailable handler.</returns>
        public IDisposable TrySubscribeWindowStateChanged(EventHandler<WindowStateChangedEventArgs> stateChangedHandler, Action<IBaseWindowModule>? featureUnavailableHandler);

        /// <summary>
        /// Closes associated window.
        /// </summary>
        public void Close();

        /// <summary>
        /// Closes the related window if open.
        /// </summary>
        /// <param name="result">
        /// Additional result data which can be retireved by awaiting <see cref="Task"/>
        /// returnd by one of the <c>OpenAsync()</c> methods.
        /// </param>
        public void Close(object? result);
    }

    /// <summary>
    /// Provides API for managing related window.
    /// </summary>
    /// <typeparam name="T">Properties type of associated module.</typeparam>
    public interface IBaseWindowControl<T> : IBaseWindowControl where T : class, new()
    {
        /// <inheritdoc/>
        IBaseWindowModule IBaseWindowControl.Module => Module;

        /// <summary>
        /// Returns related <see cref="IBaseWindowModule{T}"/> instance.
        /// </summary>
        new public IBaseWindowModule<T> Module { get; }

        /// <summary>
        /// Provides additional data for module customization.
        /// </summary>
        T Properties { get; }

        /// <summary>
        /// Enqueue customization delegate to be called during initialization time.
        /// </summary>
        /// <param name="customizeDelegate">Customization delegate.</param>
        public void Customize(Action<T> customizeDelegate);
    }
}