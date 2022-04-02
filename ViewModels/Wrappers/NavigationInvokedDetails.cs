using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Wrappers
{
    public class NavigationInvokedDetails
    {
        public ApplicationPage? InvokedPage { get; init; }

        public bool IsSettingsInvoked { get; init; }
    }
}
