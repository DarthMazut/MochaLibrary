using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx.Extensions
{
    /// <summary>
    /// Provides a set of properties used to customize dialogs for folder browsing.
    /// </summary>
    public class BrowseFolderDialogProperties
    {
        /// <summary>
        /// Title of dialog window.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Specifies directory where browsing begins.
        /// </summary>
        public string? InitialDirectory { get; set; } // Special folder ??

        /// <summary>
        /// Contains a path selected as a result of dialog interaction.
        /// </summary>
        public string? SelectedPath { get; set; }
    }
}
