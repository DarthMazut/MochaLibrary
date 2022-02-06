using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx.Extensions
{
    /// <summary>
    /// Base class for properties types of browsing dialogs.
    /// </summary>
    public abstract class BrowseDialogProperties : DialogProperties
    {
        /// <summary>
        /// Title of dialog window.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Path selected as a result of interaction with file dialog.
        /// </summary>
        public string? SelectedPath { get; set; }

        /// <summary>
        /// Specifies directory where browsing begins.
        /// </summary>
        public string? InitialDirectory { get; set; }
    }
}
