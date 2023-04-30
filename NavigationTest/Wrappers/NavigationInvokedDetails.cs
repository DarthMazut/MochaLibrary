using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationTest.Wrappers
{
    public class NavigationInvokedDetails
    {
        public AppPage? InvokedPage { get; init; }

        public bool IsSettingsInvoked { get; init; }
    }
}
