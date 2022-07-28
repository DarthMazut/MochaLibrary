using System.Threading.Tasks;

namespace MochaCore.Settings
{
    /// <summary>
    /// Serves as a base interface for <see cref="ISettingsSectionProvider{T}"/>.
    /// </summary>
    public interface ISettingsSectionProviderBase
    {
        /// <summary>
        /// Restores section to its default values.
        /// </summary>
        void RestoreDefaults();
    }
}
