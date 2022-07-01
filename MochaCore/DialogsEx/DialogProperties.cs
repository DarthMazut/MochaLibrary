using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    /// <summary>
    /// Base class for dialog properties objects.
    /// </summary>
    public class DialogProperties
    {
        /// <summary>
        /// Allows to provide additional, custom properties, which hasn't been 
        /// included in statically typed dialog properties object.
        /// </summary>
        public IDictionary<string, object> CustomProperties { get; set; } = new Dictionary<string, object>();
    }
}
