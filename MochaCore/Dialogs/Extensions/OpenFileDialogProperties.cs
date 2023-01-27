using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs.Extensions
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

        /// <summary>
        /// Tries to set <see cref="InitialDirectory"/> by mapping provided <see cref="Environment.SpecialFolder"/>
        /// value to <see cref="string"/> path. Returns <see langword="true"/> if value was
        /// mapped successfully or <see langword="false"/> if <see cref="string"/> path could
        /// not be resolved.
        /// </summary>
        /// <param name="initialDirectory">Enum value to be mapped to <see cref="string"/> path.</param>
        public bool TrySetInitialDirectory(Environment.SpecialFolder initialDirectory)
        {
            string path = Environment.GetFolderPath(initialDirectory);
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            InitialDirectory = path;
            return true;
        }

        /// <summary>
        /// Returns current value of <see cref="InitialDirectory"/> as <see cref="Environment.SpecialFolder"/>
        /// value. Returns <see langword="null"/> if <see cref="Environment.SpecialFolder"/> value could not be
        /// determined.
        /// </summary>
        public Environment.SpecialFolder? TryGetInitialDirectoryAsSpecialFolder()
        {
            return ((Environment.SpecialFolder[])Enum.GetValues(typeof(Environment.SpecialFolder)))
                .Where(s => Environment.GetFolderPath(s).Equals(InitialDirectory, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }

    }
}
