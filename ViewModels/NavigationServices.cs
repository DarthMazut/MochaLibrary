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
        public static NavigationService MainNavigationService { get; } = new NavigationService();
    }
}
