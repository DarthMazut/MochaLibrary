using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Events.Extensions
{
    /// <summary>
    /// Provides arguments for application closing event.
    /// </summary>
    public class AppClosingEventArgs : EventArgs
    {
        /// <summary>
        /// If set to <see langword="true"/> application won't close.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
