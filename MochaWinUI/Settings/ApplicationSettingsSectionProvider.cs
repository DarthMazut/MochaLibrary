using MochaCore.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace MochaWinUI.Settings
{
    /// <summary>
    /// Provides implementation of persistent storage settings section provider for WinUI 3.
    /// </summary>
    /// <typeparam name="T">Type of settings section provider.</typeparam>
    public class ApplicationSettingsSectionProvider<T> : ISettingsSectionProvider<T> where T : ISettingsSection, new()
    {
        private readonly StorageFolder _settingsFolder;
        private readonly string _settingsFileName;

        private string? _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettingsSectionProvider{T}"/> class.
        /// </summary>
        public ApplicationSettingsSectionProvider() : this(typeof(T).Name) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettingsSectionProvider{T}"/> class.
        /// </summary>
        /// <param name="settingsName">The name of the file containing the settings.</param>
        public ApplicationSettingsSectionProvider(string settingsName) : this(settingsName, ApplicationData.Current.LocalFolder) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettingsSectionProvider{T}"/> class.
        /// </summary>
        /// <param name="settingsName">The name of the file containing the settings.</param>
        /// <param name="settingsFolder">Path to the folder where settings file will reside.</param>
        public ApplicationSettingsSectionProvider(string settingsName, StorageFolder settingsFolder)
        {
            _settingsFileName = settingsName;
            _settingsFolder = settingsFolder;
        }

        /// <summary>
        /// Determines default loading strategy for <see cref="LoadAsync()"/> method.
        /// </summary>
        public LoadingMode DefaultLoadingMode { get; init; } = LoadingMode.FromCache;

        /// <summary>
        /// Determines default saving strategy for <see cref="SaveAsync(T)"/> method.
        /// </summary>
        public SavingMode DefaultSavingMode { get; init; } = SavingMode.ToOriginalSourceIfCacheChanged;

        /// <inheritdoc/>
        public Task<T> LoadAsync()
        {
            return LoadAsync(DefaultLoadingMode);
        }

        /// <inheritdoc/>
        public async Task<T> LoadAsync(LoadingMode mode)
        {
            if (mode == LoadingMode.FromCache && _cache is not null)
            {
                T cachedSettings = new();
                await cachedSettings.FillValuesAsync(_cache);
                return cachedSettings;
            }

            IStorageItem storageItem = await _settingsFolder.TryGetItemAsync(_settingsFileName);
            
            if (storageItem is StorageFile foundStorageFile)
            {
                string fileContent = await FileIO.ReadTextAsync(foundStorageFile);
                T loadedSettings = new();
                _cache = fileContent;
                await loadedSettings.FillValuesAsync(fileContent);
                return loadedSettings;
            }

            return await RestoreDefaultsAsync(SavingMode.ToOriginalSource);
        }

        /// <inheritdoc/>
        public Task SaveAsync(T settings)
        {
            return SaveAsync(settings, DefaultSavingMode);
        }

        /// <inheritdoc/>
        public async Task SaveAsync(T settings, SavingMode mode)
        {
            string serializedObject = await settings.SerializeAsync();
            bool isCacheDifferent = _cache != serializedObject;

            if (isCacheDifferent)
            {
                _cache = serializedObject;
                if (mode == SavingMode.ToOriginalSourceIfCacheChanged)
                {
                    await SaveToFile(serializedObject);
                }
            }

            if (mode == SavingMode.ToOriginalSource)
            {
                await SaveToFile(serializedObject);
            }
        }

        /// <inheritdoc/>
        public Task UpdateAsync(Action<T> updateAction)
        {
            return UpdateAsync(updateAction, DefaultLoadingMode, DefaultSavingMode);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Action<T> updateAction, LoadingMode loadingMode, SavingMode savingMode)
        {
            T currentSettings = await LoadAsync(loadingMode);
            updateAction.Invoke(currentSettings);
            await SaveAsync(currentSettings, savingMode);
        }

        /// <inheritdoc/>
        public Task<T> RestoreDefaultsAsync()
        {
            return RestoreDefaultsAsync(DefaultSavingMode);
        }

        /// <inheritdoc/>
        public async Task<T> RestoreDefaultsAsync(SavingMode mode)
        {
            T newSetting = new();
            await SaveAsync(newSetting, mode);
            return newSetting;
        }

        private async Task SaveToFile(string serializedObject)
        {
            StorageFile? storageFile = await _settingsFolder.TryGetItemAsync(_settingsFileName) as StorageFile;
            if (storageFile is null)
            {
                storageFile = await _settingsFolder.CreateFileAsync(_settingsFileName);
            }

            await FileIO.WriteTextAsync(storageFile, serializedObject);
        }
    }
}
