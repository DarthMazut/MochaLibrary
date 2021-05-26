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
        /// Active <see cref="INavigationModule"/> at the time navigation was requested.
        /// </summary>
        public INavigationModule PreviousModule { get; internal set; }

        /// <summary>
        /// <see cref="INavigationModule"/> which requested a navigation transition.
        /// </summary>
        public INavigationModule CallingModule { get; internal set; }

        /// <summary>
        /// Target of navigation process.
        /// </summary>
        public INavigationModule RequestedModule { get; internal set; }

        /// <summary>
        /// If set to <see langword="true"/> unconditionally prevents from loading cached 
        /// <see cref="INavigationModule"/> instances.
        /// </summary>
        public bool IgnoreCached { get; internal set; }

        /// <summary>
        /// An extra data object used to pass information between <see cref="INavigatable"/>
        /// objects that take part in navigation transition.
        /// </summary>
        public object Data { get; internal set; }

        /// <summary>
        /// Determines whether <see cref="CallingModule"/> will be cached.
        /// </summary>
        public bool SaveCurrent { get; internal set; }

        internal NavigationData() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationData"/>class.
        /// </summary>
        /// <param name="requestedModule">Target of navigation process.</param>
        /// <param name="data">
        /// An extra data object used to pass information between <see cref="INavigatable"/>
        /// objects that take part in navigation transition.
        /// </param>
        /// <param name="saveCurrent">Determines whether current <see cref="INavigationModule"/> will be cached.</param>
        /// <param name="ignoreCached">
        /// If set to <see langword="true"/> unconditionally prevents from loading cached 
        /// <see cref="INavigationModule"/> instances.
        /// </param>
        public NavigationData(INavigationModule requestedModule, object data, bool saveCurrent, bool ignoreCached)
        {
            RequestedModule = requestedModule;
            Data = data;
            SaveCurrent = saveCurrent;
            IgnoreCached = ignoreCached;
        }
    }
}
