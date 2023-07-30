using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Describes strategy used to handle navigation request.
    /// </summary>
    public enum NavigationType
    {
        /// <summary>
        /// Adds new navigation item onto the navigation stack. If currently active item is not
        /// the top element, the elements above current element are removed.
        /// </summary>
        Push,

        /// <summary>
        /// Moves pointer to currently active item back.
        /// </summary>
        Back,

        /// <summary>
        /// Moves pointer to currently active item forward.
        /// </summary>
        Forward,

        /// <summary>
        /// Adds new navigation item onto the navigation stack and marks current item as "modal source". 
        /// If currently active item is not the top element, the elements above current element are removed.
        /// </summary>
        PushModal,

        /// <summary>
        /// Removes all items above most recent "modal source".
        /// </summary>
        Pop
    }
}
