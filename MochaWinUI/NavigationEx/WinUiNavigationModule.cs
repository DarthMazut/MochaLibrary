using Microsoft.UI.Xaml;
using MochaCore.NavigationEx;
using MochaCore.NavigationEx.Extensions;
using System;

namespace MochaWinUI.NavigationEx
{
    public class WinUiNavigationModule : BaseNavigationModule
    {
        public WinUiNavigationModule(string id, Func<object> viewBuilder, Type viewType, Func<INavigatable>? dataContextBuilder, Type dataContextType, NavigationModuleLifecycleOptions lifecycleOptions)
            : base(id, viewBuilder, viewType, dataContextBuilder, dataContextType, lifecycleOptions) { }

        public WinUiNavigationModule(object view, INavigatable dataContext) : base(view, dataContext) { }

        public override INavigatable? GetDataContext(object view)
        {
            if (view is FrameworkElement typedView)
            {
                return (INavigatable?)typedView.DataContext;
            }

            throw new ArgumentException("Data context can only be retrieved from *FrameworkElement* or its descendant.");
        }

        public override void SetDataContext(object view, INavigatable? dataContext)
        {
            if (view is FrameworkElement typedView)
            {
                typedView.DataContext = dataContext;
                return;
            }

            throw new ArgumentException("Data context can only be set on *FrameworkElement* or its descendant.");
        }
    }
}
