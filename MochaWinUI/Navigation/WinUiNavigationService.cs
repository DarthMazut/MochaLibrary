using Microsoft.UI.Xaml;
using MochaCore.Navigation;
using MochaCore.Navigation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.Navigation
{
    /// <summary>
    /// Provides implementation of <see cref="NavigationService"/> for WinUI.
    /// </summary>
    public class WinUiNavigationService : FluentNavigationService<FrameworkElement>
    {
        protected override INavigationLifecycleModule CreateModule<TView, TDataContext>(string id, Func<TView> viewBuilder, Func<TDataContext>? dataContextBuilder, NavigationModuleLifecycleOptions? lifecycleOptions)
            => new WinUiNavigationModule<TView, TDataContext>(id, viewBuilder, dataContextBuilder, lifecycleOptions);
    }
}
