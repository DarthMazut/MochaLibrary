using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public static class NavigationServices
    {
        public static INavigationService MainNavigationService => NavigationManager.FetchNavigationService(MainNavigationServiceId);

        public static string MainNavigationServiceId => nameof(MainNavigationServiceId);
    }
}
