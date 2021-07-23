using System;
using System.Threading.Tasks;

namespace MochaCore.Navigation
{
    /// <summary>
    /// Provides abstraction for <see cref="NavigationService"/> implementation.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Returns a currently active <see cref="INavigationModule"/> object.
        /// </summary>
        INavigationModule? CurrentView { get; }

        /// <summary>
        /// Fires every time a navigation action has been requested by one of the <see cref="INavigatable"/> object.
        /// </summary>
        event EventHandler<NavigationData>? NavigationRequested;

        /// <summary>
        /// Checks whether given type it cached.
        /// </summary>
        /// <param name="module">Type to be checked.</param>
        bool CheckCached(INavigationModule module);

        /// <summary>
        /// Clears every cached <see cref="INavigatable"/> objects.
        /// </summary>
        void ClearCached();

        /// <summary>
        /// Removes cached <see cref="INavigatable"/> object of given type. 
        /// Returns <see langword="true"/> if specified type was found and removed, otherwise <see langword="false"/>.
        /// </summary>
        /// <param name="module">Type to be found and removed from cache.</param>
        bool ClearCached(INavigationModule module);

        /// <summary>
        /// Handles a navigation requests.
        /// </summary>
        /// <param name="navigationData">Essential data for navigation process.</param>
        Task<NavigationResultData> RequestNavigation(NavigationData navigationData);
    }
}