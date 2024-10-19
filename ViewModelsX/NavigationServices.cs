﻿using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX
{
    public static class NavigationServices
    {
        public static INavigationService MainNavigationService => NavigationManager.FetchNavigationService(MainNavigationServiceId);

        public static string MainNavigationServiceId => "MainNavigationService";

        public static INavigationService InternalNavigationService => NavigationManager.FetchNavigationService(InternalNavigationServiceId);

        public static string InternalNavigationServiceId => "InternalNavigationService";
    }
}
