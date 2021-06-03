using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Events.Extensions
{
    /// <summary>
    /// Represents arguments for <see cref="IEventProvider.AppClosing"/> event.
    /// </summary>
    public class AppClosingEventArgs : EventArgs
    {
        /// <summary>
        /// If set to <see langword="true"/> allows to discard event effects.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
