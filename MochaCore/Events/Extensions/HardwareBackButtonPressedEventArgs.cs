using System;

namespace MochaCore.Events.Extensions
{
    /// <summary>
    /// Represents arguments for hardware back button pressed event.
    /// </summary>
    public class HardwareBackButtonPressedEventArgs : BaseEventArgs
    {
        /// <summary>
        /// If set to <see langword="true"/> allows to discard event effects.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HardwareBackButtonPressedEventArgs"/> class.
        /// </summary>
        /// <param name="rawArgs">Event arguments from original event.</param>
        public HardwareBackButtonPressedEventArgs(EventArgs rawArgs) : base(rawArgs) { }
    }
}
