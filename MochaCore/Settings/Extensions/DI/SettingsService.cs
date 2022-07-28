namespace MochaCore.Settings.Extensions.DI
{
    /// <summary>
    /// Provides implementation of <see cref="ISettingsService"/>.
    /// </summary>
    public class SettingsService : ISettingsService
    {
        /// <inheritdoc/>
        public ISettingsSectionProvider<T> Retrieve<T>(string id) where T : ISettingsSection, new()
        {
            return SettingsManager.Retrieve<T>(id);
        }
    }
}
