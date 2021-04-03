using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Settings
{
    /// <summary>
    /// Allows for technology-independent settings management.
    /// </summary>
    public static class SettingsManager
    {
        private static readonly Dictionary<string, ISettingsSectionBase> _sections = new Dictionary<string, ISettingsSectionBase>();

        /// <summary>
        /// Registers a settings section which can be later used by
        /// technology-independent code base.
        /// </summary>
        /// <typeparam name="T">Type of settings section.</typeparam>
        /// <param name="id">Identifier of registering section.</param>
        /// <param name="section">Section to be registerd.</param>
        public static void Register<T>(string id, ISettingsSection<T> section) where T : new()
        {
            if(_sections.ContainsKey(id))
            {
                throw new ArgumentException($"Id {id} was already registered");
            }

            _sections.Add(id, section);
        }

        /// <summary>
        /// Retrieves settings section registered with given id.
        /// </summary>
        /// <typeparam name="T">Type of settings section.</typeparam>
        /// <param name="id">Id of settings section to be retrieved.</param>
        public static ISettingsSection<T> Retrieve<T>(string id) where T : new()
        {
            if(_sections.TryGetValue(id, out ISettingsSectionBase baseSection))
            {
                if(baseSection is ISettingsSection<T> section)
                {
                    return section;
                }

                throw new ArgumentException($"Requested section was found but provided type was incorrect. Requested {typeof(T).GetType()} but actual is {baseSection.GetType()}");
            }

            throw new ArgumentException($"Cannot find settings section with id {id}");
        }
    }
}
