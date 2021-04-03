using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Settings
{
    /// <summary>
    /// Represents a single section of settings which can be
    /// stored between application lunches.
    /// </summary>
    /// <typeparam name="T">Type of this section settings.</typeparam>
    public interface ISettingsSection<T> : ISettingsSectionBase where T : new()
    {
        /// <summary>
        /// Actual settings values stored by this section.
        /// </summary>
        T Settings { get; set;}

        /// <summary>
        /// Changes the settings by invoking given delegate and then 
        /// saves them to non-volatile memory.
        /// </summary>
        /// <param name="setSettings">Delegate which changes the settings.</param>
        void Save(Action<T> setSettings);
    }
}
