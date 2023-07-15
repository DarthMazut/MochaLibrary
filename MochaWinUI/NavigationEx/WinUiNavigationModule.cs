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

    public class WinUiModule2 : BaseNavigationModule<FrameworkElement, INavigatable>
    {
        public WinUiModule2(
            string id,
            Func<FrameworkElement> viewBuilder,
            Func<INavigatable>? dataContextBuilder,
            NavigationModuleLifecycleOptions? lifecycleOptions) :
                base(id, viewBuilder, dataContextBuilder, lifecycleOptions)
        {
            INavigatable x = null;
            WinUiModule2 module = WinUiModule2.Create("", () => new UserControl(), () => x);
        }

        public static WinUiModule2 Create<TView, TDataContext>(string id, Func<TView> viewBuilder, Func<TDataContext> contextBuilder)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
        {
            return new WinUiModule2(id, viewBuilder, contextBuilder, null);
        }

        public override INavigatable? GetDataContext(FrameworkElement view)
        {
            return view.DataContext as INavigatable;
        }

        public override void SetDataContext(FrameworkElement view, INavigatable? dataContext)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Provides implementation of <see cref="INavigationModule"/> for WinUI.
    /// </summary>
    /// <typeparam name="TView">Type of view object.</typeparam>
    /// <typeparam name="TDataContext">Type of object which serves as data context for view object.</typeparam>
    public class WinUiNavigationModule<TView, TDataContext> : BaseNavigationModule<TView, TDataContext> 
        where TView : FrameworkElement
        where TDataContext : class, INavigatable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNavigationModule{TView, TDataContext}"/> class.
        /// </summary>
        /// <param name="id">Unique identifier of creating module.</param>
        /// <param name="viewBuilder">A delegate that returns new instance of the view object.</param>
        /// <param name="dataContextBuilder"></param>
        /// <param name="lifecycleOptions">Provides additional settings for managing lifecycle of <see cref="INavigationLifecycleModule"/>.</param>
        public WinUiNavigationModule(string id, Func<TView> viewBuilder, Func<TDataContext>? dataContextBuilder, NavigationModuleLifecycleOptions? lifecycleOptions) :
            base(id, viewBuilder, dataContextBuilder, lifecycleOptions) { }

        /// <inheritdoc/>
        public override TDataContext? GetDataContext(TView view)
        {
            if (view.DataContext is not null && view.DataContext is not TDataContext)
            {
                throw new ArgumentException($"{nameof(INavigationModule.DataContext)} for {nameof(TDataContext)} must be of type {nameof(TView)}", nameof(view));
            }

            return view.DataContext as TDataContext;  
        }

        /// <inheritdoc/>
        public override void SetDataContext(TView view, TDataContext? dataContext)
            => view.DataContext = dataContext;
    }
}
