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
    public class WinUiNavigationService : BaseNavigationService
    {
        public WinUiNavigationService WithModule<TView, TDataContext>(string id)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
                => WithModule<TView, TDataContext>(id, new NavigationModuleLifecycleOptions());

        public WinUiNavigationService WithModule<TView, TDataContext>(string id, NavigationModuleLifecycleOptions lifecycleOptions)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
        {
            if (typeof(TView).GetConstructor(Type.EmptyTypes) is null)
            {
                throw new ArgumentException("Provided view type does not have parameterless constructor and cannot be registered " +
                    $"by this overload of {nameof(WithModule)} method.");
            }

            if (typeof(TDataContext).GetConstructor(Type.EmptyTypes) is null)
            {
                throw new ArgumentException("Provided dataContext type does not have parameterless constructor and cannot be registered " +
                    $"by this overload of {nameof(WithModule)} method.");
            }

            AddModule(new WinUiNavigationModule(id, () => Activator.CreateInstance<TView>(), typeof(TView), () => Activator.CreateInstance<TDataContext>(), typeof(TDataContext), lifecycleOptions));
            return this;
        }

        public WinUiNavigationService WithModule<TView, TDataContext>(string id, Func<TDataContext> dataContextBuilder)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
                => WithModule<TView, TDataContext>(id, dataContextBuilder, new NavigationModuleLifecycleOptions());

        public WinUiNavigationService WithModule<TView, TDataContext>(string id, Func<TDataContext> dataContextBuilder, NavigationModuleLifecycleOptions lifecycleOptions)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
        {
            if (typeof(TView).GetConstructor(Type.EmptyTypes) is null)
            {
                throw new ArgumentException("Provided view type does not have parameterless constructor and cannot be registered " +
                    $"by this overload of {nameof(WithModule)} method.");
            }

            AddModule(new WinUiNavigationModule(id, () => Activator.CreateInstance<TView>(), typeof(TView), dataContextBuilder, typeof(TDataContext), lifecycleOptions));
            return this;
        }

        public WinUiNavigationService WithModule<TView, TDataContext>(string id, Func<TView> viewBuilder, Func<TDataContext> dataContextBuilder)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable 
                => WithModule(id, viewBuilder, dataContextBuilder, new NavigationModuleLifecycleOptions());

        public WinUiNavigationService WithModule<TView, TDataContext>(string id, Func<TView> viewBuilder, Func<TDataContext> dataContextBuilder, NavigationModuleLifecycleOptions lifecycleOptions)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
        {
            AddModule(new WinUiNavigationModule(id, viewBuilder, typeof(TView), dataContextBuilder, typeof(TDataContext), lifecycleOptions));
            return this;
        }

        public WinUiNavigationService WithRoot(Window window)
        {
            INavigatable? dataContext = (window.Content as FrameworkElement)?.DataContext as INavigatable;
            if (dataContext == null) 
            {
                throw new ArgumentException($"Cannot find data context of provided object or data context is not {nameof(INavigatable)}");
            }
            return WithRoot(window, dataContext);
        }

        public WinUiNavigationService WithRoot(Window window, INavigatable dataContext)
        {
            AddModule(new WinUiNavigationModule(window, dataContext));
            return this;
        }

        public WinUiNavigationService WithInitialId(string initialId)
        {
            SelectInitialtId(initialId);
            return this;
        }

        public async Task<WinUiNavigationService> Initialize()
        {
            await base.Initialize();
            return this;
        }
    }
}
