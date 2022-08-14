using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Settings
{
    /// <summary>
    /// Defines how the <see cref="ISettingsSectionProvider{T}"/> object
    /// handles reading values.
    /// </summary>
    public enum LoadingMode
    {
        /// <summary>
        /// Values are read from memory cache.
        /// </summary>
        FromCache,

        /// <summary>
        /// Values are read from source. This option guarantees that
        /// provided values are up to date.
        /// </summary>
        FromOriginalSource
    }

    /// <summary>
    /// Defines how the <see cref="ISettingsSectionProvider{T}"/> object
    /// handles saving values.
    /// </summary>
    public enum SavingMode
    {
        /// <summary>
        /// Values are saved within memory cache only.
        /// </summary>
        ToCacheOnly,

        /// <summary>
        /// Values are saved to its original source only
        /// if they differs from those in memory cache.
        /// </summary>
        ToOriginalSourceIfCacheChanged,

        /// <summary>
        /// Values are saved to its original source.
        /// </summary>
        ToOriginalSource
    }
}
