﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Settings.Extensions.DI
{
    /// <summary>
    /// Provides implementation of <see cref="ISettingsService"/>.
    /// </summary>
    public class SettingsService : ISettingsService
    {
        /// <inheritdoc/>
        public ISettingsSection<T> Retrieve<T>(string id) where T : class, new()
        {
            return SettingsManager.Retrieve<T>(id);
        }
    }
}
