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

        /// <summary>
        /// Returns settings assocaited with this section saved in non-volatile memory. 
        /// </summary>
        public T Load()
        {
            if (_appSettings[_settingName] == null)
            {
                _appSettings[_settingName] = new T();
            }

            return (T)_appSettings[_settingName];
        }

        /// <summary>
        /// Saves settings to non-volatile memory.
        /// </summary>
        /// <param name="settings">Settings to be saved.</param>
        public void Save(T settings)
        {
            if (settings is null) throw new ArgumentNullException();

            _appSettings[_settingName] = settings;
            _appSettings.Save();
        }

        /// <summary>
        /// Changes the settings by invoking given delegate and then 
        /// saves them to non-volatile memory.
        /// </summary>
        /// <param name="updateAction">Delegate which changes the settings.</param>
        public void Update(Action<T> updateAction)
        {
            if (updateAction is null) throw new ArgumentNullException();

            T settings = Load();
            updateAction.Invoke(settings);
            Save(settings);
        }

        /// <summary>
        /// Restores section to its default values.
        /// </summary>
        public void RestoreDefaults()
        {
            _appSettings.Reset();
        }
    }
}
