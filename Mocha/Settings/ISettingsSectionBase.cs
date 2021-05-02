using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// Asynchronously restores section to its default values.
        /// </summary>
        Task RestoreDefaultsAsync();
    }
}
