using Mocha.Navigation;
using MochaWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWPFTestApp
{
    static class NavigationModules
    {
        public static INavigationModule Page1 => NavigationManager.FetchModule(PagesIDs.Page1);
        public static INavigationModule Page2 => NavigationManager.FetchModule(PagesIDs.Page2);
        public static INavigationModule Page3 => NavigationManager.FetchModule(PagesIDs.Page3);
    }

    static class PagesIDs
    {
        public static string Page1 => "Page1";
        public static string Page2 => "Page2";
        public static string Page3 => "Page3";
    }
}
