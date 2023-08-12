using System;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Provides API form managing related window.
    /// </summary>
    public interface IWindowControl
    {
        /// <summary>
        /// Returns the technolog-specific window object.
        /// </summary>
        public object View { get; }

        /// <summary>
        /// Occurs when related window is opened.
        /// </summary>
        public event EventHandler? Opened;

        /// <summary>
        /// Occurs when related window closes.
        /// </summary>
        public event EventHandler? Closed;

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
    /// Provides API form managing related window.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public interface IWindowControl<T> : IWindowControl where T : class, new()
    {
        /// <summary>
        /// Allows for providing additional data for module customization.
        /// </summary>
        T Properties { get; }
    }
}