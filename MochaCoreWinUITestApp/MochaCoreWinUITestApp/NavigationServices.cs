﻿using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWinUITestApp
{
    public static class NavigationServices
    {
        public static INavigationService MainNavigationService { get; } = new NavigationService();
    }
}
