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
        /// <summary>
        /// Restores section to its default values.
        /// </summary>
        void RestoreDefaults();
    }
}
