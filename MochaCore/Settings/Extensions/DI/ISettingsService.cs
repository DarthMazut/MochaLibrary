namespace MochaCore.Settings.Extensions.DI
{
    /// <summary>
    /// Allows obtaining <see cref="ISettingsSection{T}"/> objects for a Dependency Injection based architecture. 
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Retrieves settings section registered with given id.
        /// </summary>
        /// <typeparam name="T">Type of settings section.</typeparam>
        /// <param name="id">Id of settings section to be retrieved.</param>
        ISettingsSection<T> Retrieve<T>(string id) where T : class, new();
    }
}
