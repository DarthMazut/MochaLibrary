using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx.Extensions
{
    /// <summary>
    /// Base class for file dialog properties types.
    /// </summary>
    public abstract class FileDialogProperties : BrowseDialogProperties
    {
        /// <summary>
        /// A collection of <see cref="ExtensionFilter"/> objectes, that 
        /// determines which file types will be visible when browsing the folder structure.
        /// </summary>
        public IList<ExtensionFilter> Filters { get; set; } = new List<ExtensionFilter>();
    }
}
