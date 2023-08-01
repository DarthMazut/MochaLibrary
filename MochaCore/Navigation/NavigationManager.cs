using MochaCore.Navigation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Navigation
{
    /// <summary>
    /// Allows for registering <see cref="INavigationService"/> instances on the technology-specific side 
    /// and later retrieval within the technology-agnostic modules.
    /// </summary>
    public static class NavigationManager
    {
        private static readonly Dictionary<string, INavigationService> _navigationServices = new();

        /// <summary>
        /// Registers the provided <see cref="INavigationService"/> instance.
        /// </summary>
        /// <param name="id">The unique identifier for registering the service.</param>
        /// <param name="navigationService">The instance to be registered.</param>
        /// <exception cref="ArgumentException">Thrown when the provided <paramref name="id"/> is not unique.</exception>
        /// <returns>The registered instance of <see cref="INavigationService"/>.</returns>
        public static INavigationService AddNavigationService(string id, INavigationService navigationService)
        {
            if (_navigationServices.ContainsKey(id))
            {
                throw new ArgumentException($"Navigation service with id *{id}* was already registered.", nameof(id));
            }

            _navigationServices.Add(id, navigationService);
            return navigationService;
        }

        /// <summary>
        /// Retrieves registered <see cref="INavigationService"/> instance by its identifier.
        /// </summary>
        /// <param name="id"><see cref="INavigationService"/> unique identifier.</param>
        /// <exception cref="ArgumentException"></exception>
        public static INavigationService FetchNavigationService(string id)
        {
            if (!_navigationServices.ContainsKey(id))
            {
                throw new ArgumentException($"Cannot retrieve navigation service with id *{id}*, because it was never registered.", nameof(id));
            }

            return _navigationServices[id];
        }
    }
}
