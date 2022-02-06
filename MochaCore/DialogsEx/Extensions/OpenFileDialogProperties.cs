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
    public class OpenFileDialogProperties : FileDialogProperties
    {
        /// <summary>
        /// Determines whether selecting multiple files is allowed.
        /// </summary>
        public bool MultipleSelection { get; set; }

        /// <summary>
        /// Contains a list of paths selected as a result of dialog interaction.
        /// </summary>
        public IList<string> SelectedPaths { get; set; } = new List<string>();
    }
}
