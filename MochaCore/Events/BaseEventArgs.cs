using System;

namespace MochaCore.Events
{
    /// <summary>
    /// Provides a base class for <see cref="EventArgs"/> objects
    /// used with <see cref="Events"/> namespace.
    /// </summary>
    public class BaseEventArgs : EventArgs
    {
        /// <summary>
        /// Contains <see cref="EventArgs"/> object from the original technology-specific event.
        /// </summary>
        public EventArgs RawArgs { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEventArgs"/> class.
        /// </summary>
        /// <param name="rawArgs">Event arguments from original event.</param>
        public BaseEventArgs(EventArgs rawArgs)
        {
            RawArgs = rawArgs;
        }
    }
}
