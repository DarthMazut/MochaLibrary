using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Provides method for initializing <see cref="IBaseWindowControl"/> instance.
    /// </summary>
    public interface IWindowControlInitialize
    {
        /// <summary>
        /// Initializes <see cref="IBaseWindowControl"/> instance.
        /// </summary>
        /// <param name="module">Module associated with initializing <see cref="IBaseWindowControl"/> instance.</param>
        public void Initialize(IBaseWindowModule module);
    }
}
