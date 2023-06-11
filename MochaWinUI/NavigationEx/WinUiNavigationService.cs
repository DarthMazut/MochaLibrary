using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MochaCore.NavigationEx;
using MochaCore.NavigationEx.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.NavigationEx
{
    /// <summary>
    /// Provides implementation of <see cref="INavigationModule"/> for WinUI.
    /// </summary>
    public class WinUiNavigationService : FluentNavigationService<FrameworkElement>
    {
        protected override INavigationLifecycleModule CreateModule<TView, TDataContext>(string id, Func<TView> viewBuilder, Func<TDataContext>? dataContextBuilder, NavigationModuleLifecycleOptions? lifecycleOptions)
        {
            return new WinUiNavigationModule<TView, TDataContext>(id, viewBuilder, dataContextBuilder, lifecycleOptions);
        }
    }
}
