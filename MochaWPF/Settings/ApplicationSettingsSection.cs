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
    public class ApplicationSettingsSection<T> : ISettingsSection<T> where T : new()
    {
        private readonly ApplicationSettingsBase _appSettings;
        private readonly string _settingName;
        private T _settings;

        /// <summary>
        /// Actual settings values stored by this section.
        /// </summary>
        // [UserScopedSetting]
        public T Settings
        {
            get
            {
                if(_appSettings[_settingName] == null)
                {
                    _appSettings[_settingName] = new T();
                }

                return (T)_appSettings[_settingName];
            }
            set => _appSettings[_settingName] = value;
        }

        /// <summary>
        /// Returns new instance of <see cref="ApplicationSettingsSection{T}"/> class.
        /// </summary>
        /// <param name="appSettings"><see cref="ApplicationSettingsBase"/> object retrieved from <AppName>.Properties.Settings.Default.</param>
        /// <param name="settingName">Settings name defined in Settings Designer.</param>
        public ApplicationSettingsSection(ApplicationSettingsBase appSettings, string settingName)
        {
            if(appSettings == null || settingName == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                _settings = (T)appSettings[settingName];
            }
            catch (Exception ex)
            {
                throw new AggregateException($"{settingName} as setting name is invalid.", ex);
            }

            _appSettings = appSettings;
            _settingName = settingName;
        }

        /// <summary>
        /// Restores section to its default values.
        /// </summary>
        public void RestoreDefault()
        {
            _appSettings.Reset();
        }

        /// <summary>
        /// Reloads values from persistent storage.
        /// </summary>
        public void Load()
        {
            _appSettings.Reload();
        }

        /// <summary>
        /// Saves settings to non-volatile memory.
        /// </summary>
        public void Save()
        {
            _appSettings.Save();
        }

        /// <summary>
        /// Changes the settings by invoking given delegate and then 
        /// saves them to non-volatile memory.
        /// </summary>
        /// <param name="setSettings">Delegate which changes the settings.</param>
        public void Save(Action<T> setSettings)
        {
            setSettings?.Invoke(Settings);
            Save();
        }


    }
}
