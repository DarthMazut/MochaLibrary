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
        /// Asynchronously returns settings assocaited with this section provider saved in non-volatile memory.
        /// This method uses default <see cref="LoadingMode"/> value specified within implementing type.
        /// </summary>
        Task<T> LoadAsync();

        /// <summary>
        /// Asynchronously returns settings assocaited with this section provider using specified strategy.
        /// </summary>
        /// <param name="mode">Defines a loading strategy.</param>
        Task<T> LoadAsync(LoadingMode mode);

        /// <summary>
        /// Asynchronously saves provided settings to non-volatile memory.
        /// This method uses default <see cref="SavingMode"/> value specified within implementing type.
        /// </summary>
        /// <param name="settings">Settings to be saved.</param>
        Task SaveAsync(T settings);

        /// <summary>
        /// Asynchronously saves provided settings to non-volatile memory using specified strategy.
        /// </summary>
        /// <param name="settings">Settings to be saved.</param>
        /// <param name="mode">Defines a saving strategy.</param>
        Task SaveAsync(T settings, SavingMode mode);

        /// <summary>
        /// Asynchronously changes the settings by invoking given delegate and then 
        /// saves them to non-volatile memory. This method uses default <see cref="SavingMode"/> and
        /// <see cref="LoadingMode"/> values specified within implementing type.
        /// </summary>
        /// <param name="updateAction">Delegate which changes the settings.</param>
        Task UpdateAsync(Action<T> updateAction);

        /// <summary>
        /// Asynchronously changes the settings by invoking given delegate and then 
        /// saves them to non-volatile memory.
        /// </summary>
        /// <param name="updateAction">Delegate which changes the settings.</param>
        /// <param name="loadingMode">Defines loading strategy.</param>
        /// <param name="savingMode">Defines a saving strategy.</param>
        Task UpdateAsync(Action<T> updateAction, LoadingMode loadingMode, SavingMode savingMode);

        /// <summary>
        /// Asynchronously restores section to its default values using default <see cref="SavingMode"/> strategy.
        /// </summary>
        Task<T> RestoreDefaultsAsync();

        /// <summary>
        /// Asynchronously restores section to its default values.
        /// </summary>
        /// <param name="mode">Defines a strategy of saving default values.</param>
        Task<T> RestoreDefaultsAsync(SavingMode mode);
    }
}
