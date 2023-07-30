using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils
{
    /// <summary>
    /// Allows for limited control over an object whose data context is set to the implementing instance.
    /// </summary>
    public interface IBindingTargetController
    {
        /// <summary>
        /// Occurs when the current instance requests the execution of a defined action.
        /// </summary>
        public event EventHandler<BindingTargetControlRequestedEventArgs>? BindingTargetControlRequested;
    }
}
