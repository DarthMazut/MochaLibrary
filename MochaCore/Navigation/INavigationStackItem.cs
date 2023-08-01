using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Navigation
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
        /// Indicates whether the current item is a source of ongoing <see cref="NavigationType.PushModal"/> navigation request.
        /// </summary>
        public bool IsModalOrigin { get; }
    }
}
