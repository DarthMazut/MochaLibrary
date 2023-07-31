using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils
{
    /// <summary>
    /// Allows for execution of specified action by UI object which subscribes to <see cref="ControlRequested"/>
    /// event of implementing instance. See <see cref="BindingTargetControlRequestType"/> for avaiable actions.
    /// </summary>
    public interface IBindingTargetController
    {
        /// <summary>
        /// Occurs when the current instance requests the execution of a specified action.
        /// </summary>
        public event EventHandler<BindingTargetControlRequestedEventArgs>? ControlRequested;
    }
}
