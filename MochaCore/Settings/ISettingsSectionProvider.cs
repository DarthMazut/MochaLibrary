using System;
using System.Threading.Tasks;

namespace MochaCore.Settings
{
    /// <summary>
    /// Provides abstraction of settings data source.
    /// </summary>
    /// <typeparam name="T">Type of provided settings data.</typeparam>
    public interface ISettingsSectionProvider<T> : ISettingsSectionProviderBase where T : ISettingsSection, new()
    {
        /// <summary>
        /// Returns settings assocaited with this section saved in non-volatile memory. 
        /// </summary>
        [Obsolete("Use asynchronous versions of this method")]
        T Load();

        /// <summary>
        /// Asynchronously returns settings assocaited with this section provider saved in non-volatile memory.
        /// </summary>
        Task<T> LoadAsync();

        /// <summary>
        /// Asynchronously returns settings associated with this section provider saved in non-volatile memory.
        /// </summary>
        /// <param name="ignoreCache">Determines whether to ignore cached settings and fetch them directly from original source.</param>
        Task<T> LoadAsync(bool ignoreCache);

        /// <summary>
        /// Saves provided settings to non-volatile memory.
        /// </summary>
        /// <param name="settings">Settings to be saved.</param>
        [Obsolete("Use asynchronous versions of this method")]
        void Save(T settings);

        /// <summary>
        /// Asynchronously saves provided settings to non-volatile memory.
        /// </summary>
        /// <param name="settings">Settings to be saved.</param>
        Task SaveAsync(T settings);

        /// <summary>
        /// Asynchronously saves provided settings to non-volatile memory.
        /// </summary>
        /// <param name="settings">Settings to be saved.</param>
        /// <param name="saveToOriginalSource">Determines whether to save provided settings to original source.
        /// <see langword="False"/> means that only cached settings are updated.</param>
        Task SaveAsync(T settings, bool saveToOriginalSource);

        /// <summary>
        /// Changes the settings by invoking given delegate and then 
        /// saves them to non-volatile memory.
        /// </summary>
        /// <param name="updateAction">Delegate which changes the settings.</param>
        [Obsolete("Use asynchronous versions of this method")]
        void Update(Action<T> updateAction);

        /// <summary>
        /// Asynchronously changes the settings by invoking given delegate and then 
        /// saves them to non-volatile memory.
        /// </summary>
        /// <param name="updateAction">Delegate which changes the settings.</param>
        Task UpdateAsync(Action<T> updateAction);

        /// <summary>
        /// Asynchronously changes the settings by invoking given delegate and then 
        /// saves them to non-volatile memory.
        /// </summary>
        /// <param name="updateAction">Delegate which changes the settings.</param>
        /// <param name="saveToOriginalSource">Determines whether to save provided settings to original source.
        /// <see langword="False"/> means that only cached settings are updated.</param>
        /// <param name="ignoreCache">Determines whether settings are fetched from cache or original source.</param>
        Task UpdateAsync(Action<T> updateAction, bool saveToOriginalSource, bool ignoreCache);

        /// <summary>
        /// Asynchronously restores section to its default values.
        /// </summary>
        Task<T> RestoreDefaultsAsync();

        /// <summary>
        /// Asynchronously restores section to its default values.
        /// </summary>
        /// <param name="affectOriginalSource">Determines whether both cached settings and original source
        /// should be set to theirs default values.</param>
        Task<T> RestoreDefaultsAsync(bool affectOriginalSource);
    }
}
