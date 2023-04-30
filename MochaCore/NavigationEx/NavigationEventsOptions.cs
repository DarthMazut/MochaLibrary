using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Allows for defining custom navigation events behaviour.
    /// </summary>
    public class NavigationEventsOptions
    {
        public bool SupressNavigatingFromEvents { get; init; }

        /// <summary>
        /// If set to <see langword="true"/> prevents <see cref="INavigationService"/>
        /// from calling <see cref="IOnNavigatedTo.OnNavigatedTo(OnNavigatedToEventArgs)"/>
        /// and <see cref="IOnNavigatedToAsync.OnNavigatedToAsync(OnNavigatedToEventArgs)"/>
        /// methods.
        /// </summary>
        public bool SupressNavigatedToEvents { get; init; }

        public bool SupressNavigatedFromEvents { get; init; }
    }
}
