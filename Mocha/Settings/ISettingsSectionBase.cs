using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Settings
{
    /// <summary>
    /// Serves as a base interface for <see cref="ISettingsSection{T}"/>.
    /// </summary>
    public interface ISettingsSectionBase
    {
        /* MAYBE IN DA FUTURE 

        /// <summary>
        /// Determines whether the settings should be saved immediately after assigning new value. 
        /// If this option is set to <see langword="false"/> use the <see cref="Save"/> method after assigning 
        /// new values to save the changes. 
        /// </summary>
        bool AutoSaving { get; set; }

        /// <summary>
        /// If <see cref="AutoSaving"/> is set to <see langword="true"/> restores last
        /// saved values.
        /// </summary>
        void DiscardChanges();

        */

        /// <summary>
        /// Reloads values from persistent storage.
        /// </summary>
        void Load();

        /// <summary>
        /// Restores section to its default values.
        /// </summary>
        void RestoreDefault();

        /// <summary>
        /// Saves settings to non-volatile memory.
        /// </summary>
        void Save();
    }
}
