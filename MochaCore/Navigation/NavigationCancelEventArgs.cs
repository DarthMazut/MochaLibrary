using System;

namespace MochaCore.Navigation
{
    /// <summary>
    /// Provides data for navigation canceling.
    /// </summary>
    public class NavigationCancelEventArgs : EventArgs
    {
        /// <summary>
        /// Specifies whether navigation should be aborted.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Reason for navigation being aborted.
        /// </summary>
        public string? Reason { get; set; }
    }
}
