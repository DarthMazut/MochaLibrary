﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public class NavigationModuleLifecycleOptions
    {
        /// <summary>
        /// Determines wheter associated <see cref="INavigationModule"/> should be cached
        /// by default. This behaviour can be overwritten by <see cref="Navigator.SaveCurrent"/> proeprty.
        /// </summary>
        public bool PreferCache { get; init; }

        /// <summary>
        /// Determines whether the associated <see cref="INavigationModule.DataContext"/> should be disposed
        /// when its module is uninitialized.
        /// </summary>
        public bool DisposeDataContextOnUninitialize { get; init; }
    }
}
