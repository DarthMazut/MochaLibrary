using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Events.Extensions
{
    /// <summary>
    /// Represents arguments for hardware back button pressed event.
    /// </summary>
    public class HardwareBackButtonPressedEventArgs : EventArgs
    {
        /// <summary>
        /// If set to <see langword="true"/> allows to discard event effects.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
