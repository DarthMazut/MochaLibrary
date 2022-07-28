using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Settings
{
    /// <summary>
    /// Allows implementing class to be provided by <see cref="ISettingsSectionProvider{T}"/> instance.
    /// Represents a single section of settings which can be stored between application lunches.
    /// </summary>
    public interface ISettingsSection
    {
        /// <summary>
        /// Returns a string representing the implementing instance.
        /// </summary>
        Task<string> SerializeAsync();

        /// <summary>
        /// Modifies state of implementing instance to reflect provided serialized <see langword="string"/>
        /// </summary>
        /// <param name="serializedData">Contains serialized data.</param>
        Task FillValuesAsync(string serializedData);
    }
}
