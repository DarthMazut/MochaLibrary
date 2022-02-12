using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx.Extensions
{
    /// <summary>
    /// Provides standard properties for customization of file save dialog.
    /// </summary>
    public class SaveFileDialogProperties : DialogProperties
    {
        /// <summary>
        /// Dialog title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Path to directory when browsing starts.
        /// </summary>
        public string? InitialDirectory { get; set; }

        /// <summary>
        /// A collection of <see cref="ExtensionFilter"/> objectes, that 
        /// determines which file types will be visible when browsing the folder structure.
        /// </summary>
        public IList<ExtensionFilter> Filters { get; set; } = new List<ExtensionFilter>();

        /// <summary>
        /// Path selected as a result of interaction with file dialog.
        /// </summary>
        public string? SelectedPath { get; set; }
    }
}
