using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWPFTestApp
{
    public static class Pages
    {
        public static string Page1Id => "Page1";
        public static string Page2Id => "Page2";

        public static INavigationModule Page1 => NavigationManager.FetchModule(Page1Id);
        public static INavigationModule Page2 => NavigationManager.FetchModule(Page2Id);
    }
}
