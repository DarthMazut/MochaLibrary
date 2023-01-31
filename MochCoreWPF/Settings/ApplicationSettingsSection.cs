using System;
using System.Configuration;
using System.Threading.Tasks;
using MochaCore.Settings;

namespace MochaWPF.Settings
{
    /// <summary>
    /// Provides WPF implementation for settings persistance storage using
    /// <see cref="ApplicationSettingsBase"/> implementation.
    /// </summary>
    /// <typeparam name="T">Type of settings section.</typeparam>
    public class ApplicationSettingsSectionProvider<T> : ISettingsSectionProvider<T> where T : ISettingsSection, new()
    {
        private readonly ApplicationSettingsBase _appSettings;
        private readonly string _settingName;

        private T? _cache;

        /// <summary>
        /// Returns new instance of <see cref="ApplicationSettingsSectionProvider{T}"/> class.
        /// </summary>
        /// <param name="appSettings"><see cref="ApplicationSettingsBase"/> object retrieved from [AppName].Properties.Settings.Default.</param>
        /// <param name="settingName">Settings name defined in Settings Designer.</param>
        public ApplicationSettingsSectionProvider(ApplicationSettingsBase appSettings, string settingName)
        {
            _ = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _ = settingName ?? throw new ArgumentNullException(nameof(settingName));

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
                return _cache;
            }

            if (_appSettings[_settingName] == null)
            {
                return await RestoreDefaultsAsync(SavingMode.ToOriginalSource);
            }

            _cache = (T)_appSettings[_settingName];
            return (T)_appSettings[_settingName];
        }

        /// <inheritdoc/>
        public Task SaveAsync(T settings)
        {
            return SaveAsync(settings, DefaultSavingMode);
        }

        /// <inheritdoc/>
        public async Task SaveAsync(T settings, SavingMode mode)
        {
            bool isCacheDifferent;
            if (_cache is null)
            {
                isCacheDifferent = true;
            }
            else
            {
                isCacheDifferent = await _cache.SerializeAsync() == await settings.SerializeAsync();
            }

            if (isCacheDifferent)
            {
                _cache = settings;
                if (mode == SavingMode.ToOriginalSourceIfCacheChanged)
                {
                    _appSettings[_settingName] = settings;
                    _appSettings.Save();
                }
            }

            if (mode == SavingMode.ToOriginalSource)
            {
                _appSettings[_settingName] = settings;
                _appSettings.Save();
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
            T newSettings = new();
            await SaveAsync(newSettings, mode);
            return newSettings;
        }
    }
}
