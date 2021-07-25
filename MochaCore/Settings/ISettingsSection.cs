using System;
using System.Threading.Tasks;

namespace MochaCore.Settings
{
    /// <summary>
    /// Represents a single section of settings which can be
    /// stored between application lunches.
    /// </summary>
    /// <typeparam name="T">Type of this section settings.</typeparam>
    public interface ISettingsSection<T> : ISettingsSectionBase where T : class, new()
    {
        /// <summary>
        /// Returns settings assocaited with this section saved in non-volatile memory. 
        /// </summary>
        T Load();

        /// <summary>
        /// Asynchronously returns settings assocaited with this section saved in non-volatile memory. 
        /// </summary>
        Task<T> LoadAsync();

        /// <summary>
        /// Saves settings to non-volatile memory.
        /// </summary>
        /// <param name="settings">Settings to be saved.</param>
        void Save(T settings);

        /// <summary>
        /// Asynchronously saves settings to non-volatile memory.
        /// </summary>
        /// <param name="settings">Settings to be saved.</param>
        Task SaveAsync(T settings);

        /// <summary>
        /// Changes the settings by invoking given delegate and then 
        /// saves them to non-volatile memory.
        /// </summary>
        /// <param name="updateAction">Delegate which changes the settings.</param>
        void Update(Action<T> updateAction);

        /// <summary>
        /// Asynchronously changes the settings by invoking given delegate and then 
        /// saves them to non-volatile memory.
        /// </summary>
        /// <param name="updateAction">Delegate which changes the settings.</param>
        Task UpdateAsync(Action<T> updateAction);
    }
}
