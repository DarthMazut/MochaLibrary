using System;

namespace MochaCore.Events.Extensions
{
    /// <summary>
    /// Provides arguments for application closing event.
    /// </summary>
    public class AppClosingEventArgs : BaseEventArgs
    {
        /// <summary>
        /// If set to <see langword="true"/> application won't close.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppClosingEventArgs"/> class.
        /// </summary>
        /// <param name="rawArgs">Event arguments from original event.</param>
        public AppClosingEventArgs(EventArgs rawArgs) : base(rawArgs) { }
    }
}
