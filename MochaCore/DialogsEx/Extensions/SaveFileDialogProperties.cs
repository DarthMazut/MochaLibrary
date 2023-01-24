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
        /// Path to directory where browsing starts.
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
