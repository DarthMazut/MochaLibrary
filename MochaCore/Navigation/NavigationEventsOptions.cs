using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Navigation
{
    /// <summary>
    /// Allows for defining custom navigation events behaviour.
    /// </summary>
    public class NavigationEventsOptions
    {
        /// <summary>
        /// If set to <see langword="true"/> prevents <see cref="INavigationService"/>
        /// from calling <see cref="IOnNavigatingFrom.OnNavigatingFrom(OnNavigatingFromEventArgs)"/>
        /// and <see cref="IOnNavigatingFromAsync.OnNavigatingFromAsync(OnNavigatingFromEventArgs)"/>
        /// methods.
        /// </summary>
        public bool SupressNavigatingFromEvents { get; init; }

        /// <summary>
        /// If set to <see langword="true"/> prevents <see cref="INavigationService"/>
        /// from calling <see cref="IOnNavigatedTo.OnNavigatedTo(OnNavigatedToEventArgs)"/>
        /// and <see cref="IOnNavigatedToAsync.OnNavigatedToAsync(OnNavigatedToEventArgs)"/>
        /// methods.
        /// </summary>
        public bool SupressNavigatedToEvents { get; init; }

        /// <summary>
        /// If set to <see langword="true"/> prevents <see cref="INavigationService"/>
        /// from calling <see cref="IOnNavigatedFrom.OnNavigatedFrom(OnNavigatedFromEventArgs)"/>
        /// and <see cref="IOnNavigatedFromAsync.OnNavigatedFromAsync(OnNavigatedFromEventArgs)"/>
        /// methods.
        /// </summary>
        public bool SupressNavigatedFromEvents { get; init; }
    }
}
