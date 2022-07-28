using MochaCore.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace MochaCoreWinUI.Settings
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
        /// Determines default behaviour of <see cref="LoadAsync"/> method. Setting this to <see langword="true"/>
        /// means that settings are by default loaded from file while ignoring cache.
        /// </summary>
        public bool IgnoreLoadCache { get; init; }

        /// <summary>
        /// Determines default behaviour of <see cref="SaveAsync(T)"/> method. Setting this to <see langword="true"/>
        /// means that by default settings are not saved into file, but rather only cache is updated.
        /// </summary>
        public bool UseSaveCache { get; init; }

        /// <inheritdoc/>
        public Task<T> LoadAsync()
        {
            return LoadAsync(IgnoreLoadCache);
        }

        /// <inheritdoc/>
        public async Task<T> LoadAsync(bool ignoreCache)
        {
            if (ignoreCache is false && _cache is not null)
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
                await loadedSettings.FillValuesAsync(fileContent);
                return loadedSettings;
            }

            StorageFile newStorageFile = await _settingsFolder.CreateFileAsync(_settingsFileName);
            T newSettings = new();
            string serializedObject = await newSettings.SerializeAsync();
            await FileIO.WriteTextAsync(newStorageFile, serializedObject);
            return newSettings;
        }

        /// <inheritdoc/>
        public Task SaveAsync(T settings)
        {
            return SaveAsync(settings, !UseSaveCache);
        }

        /// <inheritdoc/>
        public async Task SaveAsync(T settings, bool saveToOriginalSource)
        {
            string serializedObject = await settings.SerializeAsync();
            _cache = serializedObject;

            if (saveToOriginalSource)
            {
                StorageFile? storageFile = await _settingsFolder.TryGetItemAsync(_settingsFileName) as StorageFile;
                if (storageFile is null)
                {
                    storageFile = await _settingsFolder.CreateFileAsync(_settingsFileName);
                }

                await FileIO.WriteTextAsync(storageFile, serializedObject);
            }
        }

        /// <inheritdoc/>
        public Task UpdateAsync(Action<T> updateAction)
        {
            return UpdateAsync(updateAction, !UseSaveCache, IgnoreLoadCache);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Action<T> updateAction, bool saveToOriginalSource, bool ignoreCache)
        {
            T currentSettings = await LoadAsync(ignoreCache);
            updateAction.Invoke(currentSettings);
            await SaveAsync(currentSettings, saveToOriginalSource);
        }

        /// <inheritdoc/>
        public Task<T> RestoreDefaultsAsync()
        {
            return RestoreDefaultsAsync(!UseSaveCache);
        }

        /// <inheritdoc/>
        public async Task<T> RestoreDefaultsAsync(bool affectOriginalSource)
        {
            T newSetting = new();
            string serializedObject = await newSetting.SerializeAsync();
            _cache = serializedObject;

            if (affectOriginalSource)
            {
                IStorageItem storageItem = await _settingsFolder.TryGetItemAsync(_settingsFileName);
                if (storageItem is StorageFile foundStorageFile)
                {
                    await FileIO.WriteTextAsync(foundStorageFile, serializedObject);
                }
                else
                {
                    StorageFile newStorageFile = await _settingsFolder.CreateFileAsync(_settingsFileName);
                    await FileIO.WriteTextAsync(newStorageFile, serializedObject);
                }
            }

            return newSetting;
        }

        public T Load()
        {
            throw new NotSupportedException();
        }

        public void RestoreDefaults()
        {
            throw new NotSupportedException();
        }

        public void Save(T settings)
        {
            throw new NotSupportedException();
        }

        public void Update(Action<T> updateAction)
        {
            throw new NotSupportedException();
        }
    }
}
