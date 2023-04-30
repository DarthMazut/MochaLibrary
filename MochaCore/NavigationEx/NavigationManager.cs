using MochaCore.NavigationEx.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public static class NavigationManager
    {
        private static readonly Dictionary<string, INavigationService> _navigationServices = new();

        public static INavigationService AddNavigationService(string id, INavigationService navigationService)
        {
            if (_navigationServices.ContainsKey(id))
            {
                throw new ArgumentException($"Navigation service with id *{id}* was already registered.", nameof(id));
            }

            _navigationServices.Add(id, navigationService);
            return navigationService;
        }

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
