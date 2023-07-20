using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Provides an abstraction for a single item of the <see cref="IReadOnlyNavigationStack{T}"/> implementation.
    /// </summary>
    public interface INavigationStackItem
    {
        /// <summary>
        /// Module associated with current item.
        /// </summary>
        public INavigationModule Module { get; }

        /// <summary>
        /// Indicates whether the <see cref="NavigationType.PushModal"/> navigation was requested
        /// while this item was active.
        /// </summary>
        public bool IsModalOrigin { get; }
    }
}
