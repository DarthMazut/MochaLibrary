using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs.Extensions
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
        public string? InitialDirectory { get; set; }

        /// <summary>
        /// Contains a path selected as a result of dialog interaction.
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
