using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx.Extensions
{
    /// <summary>
    /// Provides a set of properties used to customize dialogs for file opening. 
    /// </summary>
    public class OpenFileDialogProperties : DialogProperties
    {
        /// <summary>
        /// Title of dialog window.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Specifies directory where browsing begins.
        /// </summary>
        public string? InitialDirectory { get; set; }

        /// <summary>
        /// A collection of <see cref="ExtensionFilter"/> objectes, that 
        /// determines which file types will be visible when browsing the folder structure.
        /// </summary>
        public IList<ExtensionFilter> Filters { get; set; } = new List<ExtensionFilter>();

        /// <summary>
        /// Contains a list of paths selected as a result of dialog interaction.
        /// </summary>
        public IList<string> SelectedPaths { get; set; } = new List<string>();

        /// <summary>
        /// Determines whether selecting multiple files is allowed.
        /// </summary>
        public bool MultipleSelection { get; set; }

    }
}
