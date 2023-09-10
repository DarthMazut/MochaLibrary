namespace MochaCore.Windowing
{
    /// <summary>
    /// Provides arguments for <see cref="IWindowStateAware.StateChanged"/> event.
    /// </summary>
    public class WindowStateChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="windowState">The current window state at the time the 
        /// <see cref="IWindowStateAware.StateChanged"/> event is triggered.</param>
        public WindowStateChangedEventArgs(ModuleWindowState windowState)
        {
            WindowState = windowState;
        }

        /// <summary>
        /// Current window state.
        /// </summary>
        public ModuleWindowState WindowState { get; }
    }
}