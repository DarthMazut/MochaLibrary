using System.Threading.Tasks;

namespace MochaCore.Settings
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
