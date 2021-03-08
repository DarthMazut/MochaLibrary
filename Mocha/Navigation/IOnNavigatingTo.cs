using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mocha.Navigation
{
    /// <summary>
    /// Allows implementing class to expose an asynchronous version of
    /// *OnNavigatingTo* method which is called by <see cref="NavigationService"/>.
    /// </summary>
    public interface IOnNavigatingTo
    {
        /// <summary>
        /// Called when navigation was requested to the owner of this method. At this point navigation
        /// can be rejected.
        /// </summary>
        /// <param name="navigationData">Details of navigation request.</param>
        /// <param name="e">Allows for rejecting current navigation process.</param>
        Task OnNavigatingTo(NavigationData navigationData, NavigationCancelEventArgs e);
    }
}
