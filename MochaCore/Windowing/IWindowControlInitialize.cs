using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Provides method for initializing <see cref="IWindowControl"/> instance.
    /// </summary>
    public interface IWindowControlInitialize
    {
        /// <summary>
        /// Initializes <see cref="IWindowControl"/> instance.
        /// </summary>
        /// <param name="module">Module associated with initializing <see cref="IWindowControl"/> instance.</param>
        public void Initialize(IWindowModule module);
    }
}
