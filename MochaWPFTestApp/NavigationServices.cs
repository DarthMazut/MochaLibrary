using Mocha.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWPFTestApp
{
    static class NavigationServices
    {
        public static NavigationService MainNavigationService { get; }

        static NavigationServices()
        {
            MainNavigationService = new NavigationService();
        }
    }
}
