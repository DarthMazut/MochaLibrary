using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocha.Navigation
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
        public string Reason { get; set; }
    }
}
