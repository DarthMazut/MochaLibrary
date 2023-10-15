using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Marks implementing type as able to expose window closing event.
    /// </summary>
    public interface IClosingWindow
    {
        /// <summary>
        /// Occurs when related window is about to be close.
        /// </summary>
        public event EventHandler<CancelEventArgs>? Closing;
    }
}
