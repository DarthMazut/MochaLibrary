using Microsoft.UI.Xaml;
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
