namespace MochaCore.Settings.Extensions.DI
{
    /// <summary>
    /// Allows obtaining <see cref="ISettingsSectionProvider{T}"/> objects for a Dependency Injection based architecture. 
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Retrieves settings section provider registered with given id.
        /// </summary>
        /// <typeparam name="T">Type of settings section provider.</typeparam>
        /// <param name="id">Id of settings section provider to be retrieved.</param>
        ISettingsSectionProvider<T> Retrieve<T>(string id) where T : ISettingsSection, new();
    }
}
