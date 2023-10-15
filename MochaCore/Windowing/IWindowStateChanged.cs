using System;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Marks implementing type as able to expose window state changed event.
    /// </summary>
    public interface IWindowStateChanged
    {
        /// <summary>
        /// Occurs when related window's state changed.
        /// Window state gets changed when it's minimized, maximized, restore or hidden.
        /// </summary>
        public event EventHandler<WindowStateChangedEventArgs>? StateChanged;
    }
}