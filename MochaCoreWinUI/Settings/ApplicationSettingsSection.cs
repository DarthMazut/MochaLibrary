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
    public class ApplicationSettingsSection<T> : ISettingsSection<T> where T : class, new()
    {
        private readonly StorageFolder _settingsFolder = ApplicationData.Current.LocalFolder;
        private readonly string _settingName;
        private readonly Func<T, Task<string>> _serializationDelegate;
        private readonly Func<string, Task<T>> _deserializationDelegate;

        public ApplicationSettingsSection(Func<T, Task<string>> serializationDelegate, Func<string, Task<T>> deserializationDelegate, string settingName)
        {
            _serializationDelegate = serializationDelegate;
            _deserializationDelegate = deserializationDelegate;
            _settingName = settingName;
        }

        /// <summary>
        /// SYNC API WILL BE REMOVED FROM MOCHALIB.
        /// </summary>
        public T Load()
        {
            throw new NotSupportedException();
        }

        // Add thread-safety
        // Throw when delegate fails (InnerException)

        public async Task<T> LoadAsync()
        {
            IStorageItem storageItem = await _settingsFolder.TryGetItemAsync(_settingName);
            if (storageItem is StorageFile foundStorageFile)
            {
                string fileContent = await FileIO.ReadTextAsync(foundStorageFile);
                return await _deserializationDelegate.Invoke(fileContent);
            }

            StorageFile newStorageFile = await _settingsFolder.CreateFileAsync(_settingName);
            T settingObject = new();
            string serializedObject = await _serializationDelegate.Invoke(settingObject);
            await FileIO.WriteTextAsync(newStorageFile, serializedObject);
            return settingObject;
        }

        public void RestoreDefaults()
        {
            throw new NotSupportedException();
        }

        public Task RestoreDefaultsAsync()
        {
            throw new NotImplementedException();
        }

        public void Save(T settings)
        {
            throw new NotSupportedException();
        }

        public Task SaveAsync(T settings)
        {
            throw new NotImplementedException();
        }

        public void Update(Action<T> updateAction)
        {
            throw new NotSupportedException();
        }

        public Task UpdateAsync(Action<T> updateAction)
        {
            throw new NotImplementedException();
        }
    }
}
