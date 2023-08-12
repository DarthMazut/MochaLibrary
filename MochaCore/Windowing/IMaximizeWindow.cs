using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Marks implementing type as able to perform maximalization operation.
    /// </summary>
    public interface IMaximizeWindow
    {
        /// <summary>
        /// Maximizes related window.
        /// </summary>
        public void Maximize();
    }
}
