using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocha.Navigation
{
    /// <summary>
    /// Contains essential data for navigation process.
    /// </summary>
    public class NavigationData
    {
        /// <summary>
        /// Active <see cref="NavigationModule"/> at the time navigation was requested.
        /// </summary>
        public INavigationModule PreviousModule { get; set; }

        /// <summary>
        /// <see cref="NavigationModule"/> which requested a navigation transition.
        /// </summary>
        public INavigationModule CallingModule { get; set; }

        /// <summary>
        /// Target of navigation process.
        /// </summary>
        public INavigationModule RequestedModule { get; set; }

        /// <summary>
        /// If set to <see langword="true"/> unconditionally prevents from loading cached 
        /// <see cref="NavigationModule"/> instances.
        /// </summary>
        public bool IgnoreCached { get; set; }

        /// <summary>
        /// An extra data object used to pass information between <see cref="INavigatable"/>
        /// objects that take part in navigation transition.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Determines whether <see cref="CallingModule"/> will be cached.
        /// </summary>
        public bool SaveCurrent { get; set; } 
    }
}
