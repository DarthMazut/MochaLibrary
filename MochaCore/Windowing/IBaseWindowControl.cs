using System;
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
    }
}