using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mocha.Settings;

namespace MochaWPF.Settings
{
    /// <summary>
    /// Provides WPF implementation for settings persistance storage using
    /// <see cref="ApplicationSettingsBase"/> implementation.
    /// </summary>
    /// <typeparam name="T">Type of settings section.</typeparam>
    public class ApplicationSettingsSection<T> : ISettingsSection<T> where T : class, new()
    {
        private readonly object _syncLock = new object();
        private readonly ApplicationSettingsBase _appSettings;
        private readonly string _settingName;

        /// <summary>
        /// Returns new instance of <see cref="ApplicationSettingsSection{T}"/> class.
        /// </summary>
        /// <param name="appSettings"><see cref="ApplicationSettingsBase"/> object retrieved from [AppName].Properties.Settings.Default.</param>
        /// <param name="settingName">Settings name defined in Settings Designer.</param>
        public ApplicationSettingsSection(ApplicationSettingsBase appSettings, string settingName)
        {
            if(appSettings == null || settingName == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                _ = (T)appSettings[settingName];
            }
            catch (Exception ex)
            {
                throw new AggregateException($"{settingName} as setting name is invalid.", ex);
            }

            _appSettings = appSettings;
            _settingName = settingName;
        }

        /// <inheritdoc/>
        public T Load()
        {
            if (_appSettings[_settingName] == null)
            {
                _appSettings[_settingName] = new T();
            }

            return (T)_appSettings[_settingName];
        }

        /// <inheritdoc/>
        public void Save(T settings)
        {
            if (settings is null) throw new ArgumentNullException();

            _appSettings[_settingName] = settings;
            _appSettings.Save();
        }

        /// <inheritdoc/>
        public void Update(Action<T> updateAction)
        {
            if (updateAction is null) throw new ArgumentNullException();

            T settings = Load();
            updateAction.Invoke(settings);
            Save(settings);
        }

        /// <inheritdoc/>
        public void RestoreDefaults()
        {
            _appSettings.Reset();
        }

        /// <inheritdoc/>
        public Task RestoreDefaultsAsync()
        {
            return Task.Run(() => 
            {
                lock (_syncLock)
                {
                    RestoreDefaults();
                }
            });
        }

        /// <inheritdoc/>
        public Task<T> LoadAsync()
        {
            return Task.Run(() =>
            {
                lock (_syncLock)
                {
                    return Load();
                }
            });
        }

        /// <inheritdoc/>
        public Task SaveAsync(T settings)
        {
            return Task.Run(() => 
            {
                lock (_syncLock)
                {
                    Save(settings);
                }
            });
        }

        /// <inheritdoc/>
        public Task UpdateAsync(Action<T> updateAction)
        {
            return Task.Run(() =>
            {
                lock (_syncLock)
                {
                    UpdateAsync(updateAction);
                }
            });
        }
    }
}
