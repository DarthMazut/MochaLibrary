using System;
using System.Collections.Generic;

namespace MochaCore.Settings
{
    /// <summary>
    /// Allows for technology-independent settings management.
    /// </summary>
    public static class SettingsManager
    {
        private static readonly Dictionary<string, ISettingsSectionProviderBase> _sections = new();

        /// <summary>
        /// Registers a settings section provider which can be later used by
        /// technology-independent code base.
        /// </summary>
        /// <typeparam name="T">Type of settings section provider.</typeparam>
        /// <param name="id">Identifier of registering section.</param>
        /// <param name="section">Section to be registerd.</param>
        public static void Register<T>(string id, ISettingsSectionProvider<T> section) where T : ISettingsSection, new()
        {
            if(_sections.ContainsKey(id))
            {
                throw new ArgumentException($"Id {id} was already registered");
            }

            _sections.Add(id, section);
        }

        /// <summary>
        /// Retrieves settings section provider registered with given id.
        /// </summary>
        /// <typeparam name="T">Type of settings section provider.</typeparam>
        /// <param name="id">Identifier of settings section provider to be retrieved.</param>
        public static ISettingsSectionProvider<T> Retrieve<T>(string id) where T : ISettingsSection, new()
        {
            if(_sections.TryGetValue(id, out ISettingsSectionProviderBase? baseSection))
            {
                if(baseSection is ISettingsSectionProvider<T> section)
                {
                    return section;
                }

                throw new ArgumentException($"Requested section was found but provided type was incorrect. Requested {typeof(T)} but actual is {baseSection.GetType()}");
            }

            throw new ArgumentException($"Cannot find settings section with id {id}");
        }
    }
}
