﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Navigation
{
    /// <summary>
    /// Allows implementing class to expose an *OnNavigatingFrom* method 
    /// which is called by <see cref="NavigationService"/> whenever 
    /// navigation is requested to another <see cref="INavigationModule"/>.
    /// </summary>
    public interface IOnNavigatingFrom
    {
        /// <summary>
        /// Called whenever <see cref="INavigationModule"/> that holds this <see cref="INavigatable"/> instance is currently 
        /// active and the request has been made to navigate to antoher <see cref="INavigationModule"/> instance.
        /// Currently active <see cref="INavigatable"/> can reject the navigation at this point. Any cleaning code 
        /// should be put here including unsubscribing from events.
        /// </summary>
        /// <param name="navigationData">Details of navigation request.</param>
        /// <param name="e">Allows for rejecting current navigation process.</param>
        void OnNavigatingFrom(NavigationData navigationData, NavigationCancelEventArgs e);
    }
}
