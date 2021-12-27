using MochaCore.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MochaCoreWinUI.Settings
{
    /// <summary>
    /// Provides implementation of persistent storage settings section for WinUI 3.
    /// </summary>
    /// <typeparam name="T">Type of settings section.</typeparam>
    public class ApplicationSettingsSection<T> : ISettingsSection<T> where T : class, new()
    {
        private readonly StorageFolder _settingsFolder = ApplicationData.Current.LocalFolder;
        private readonly string _sectionName;
        private readonly Func<T, Task<string>> _serializationDelegate;
        private readonly Func<string, Task<T>> _deserializationDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettingsSection{T}"/> class.
        /// </summary>
        /// <param name="sectionName">Unique name for setting section. You can pass here same value as for registration key.</param>
        /// <param name="serializationDelegate">An asynchronous delegate which translates settings section object into string.</param>
        /// <param name="deserializationDelegate">An asynchronous delegate which creates settings section object from string.</param>
        public ApplicationSettingsSection(string sectionName, Func<T, Task<string>> serializationDelegate, Func<string, Task<T>> deserializationDelegate)
        {
            _sectionName = sectionName;
            _serializationDelegate = serializationDelegate;
            _deserializationDelegate = deserializationDelegate;
        }

        /// <inheritdoc/>
        public async Task<T> LoadAsync()
        {
            IStorageItem storageItem = await _settingsFolder.TryGetItemAsync(_sectionName);
            if (storageItem is StorageFile foundStorageFile)
            {
                string fileContent = await FileIO.ReadTextAsync(foundStorageFile);
                return await _deserializationDelegate.Invoke(fileContent);
            }

            StorageFile newStorageFile = await _settingsFolder.CreateFileAsync(_sectionName);
            T settingObject = new();
            string serializedObject = await _serializationDelegate.Invoke(settingObject);
            await FileIO.WriteTextAsync(newStorageFile, serializedObject);
            return settingObject;
        }

        /// <inheritdoc/>
        public async Task RestoreDefaultsAsync()
        {
            T newSetting = new();
            string serializedObject = await _serializationDelegate.Invoke(newSetting);

            IStorageItem storageItem = await _settingsFolder.TryGetItemAsync(_sectionName);
            if (storageItem is StorageFile foundStorageFile)
            {
                await FileIO.WriteTextAsync(foundStorageFile, serializedObject);
                return;
            }

            StorageFile newStorageFile = await _settingsFolder.CreateFileAsync(_sectionName);
            await FileIO.WriteTextAsync(newStorageFile, serializedObject);
        }

        /// <inheritdoc/>
        public async Task SaveAsync(T settings)
        {
            string serializedObject = await _serializationDelegate.Invoke(settings);

            StorageFile? storageFile = await _settingsFolder.TryGetItemAsync(_sectionName) as StorageFile;
            if (storageFile is null)
            {
                storageFile = await _settingsFolder.CreateFileAsync(_sectionName);
            }

            await FileIO.WriteTextAsync(storageFile, serializedObject);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Action<T> updateAction)
        {
            T currentSettings = await LoadAsync();
            updateAction.Invoke(currentSettings);
            await SaveAsync(currentSettings);
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
