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
    /// <typeparam name="TView">Type of view object.</typeparam>
    /// <typeparam name="TDataContext">Type of object which serves as data context for view object.</typeparam>
    public sealed class WinUiNavigationModule<TView, TDataContext> : BaseNavigationModule<TView, TDataContext>
        where TView : FrameworkElement
        where TDataContext : class, INavigationParticipant
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNavigationModule{TView, TDataContext}"/> class.
        /// </summary>
        /// <param name="id">Identifier of creating module.</param>
        /// <param name="viewBuilder">
        /// A delegate that returns new instance of *TView* class.
        /// This will be invoked when creating instance receives call to its <see cref="INavigationLifecycleModule.Initialize"/> implementation.
        /// </param>
        /// <param name="dataContextBuilder">
        /// A delegate that returns new instance of *TDataContext* class.
        /// This will be invoked when creating instance receives call to its <see cref="INavigationLifecycleModule.Initialize"/> implementation.
        /// </param>
        /// <param name="lifecycleOptions">
        /// Provides additional settings for managing lifecycle of <see cref="INavigationLifecycleModule"/>.
        /// </param>
        public WinUiNavigationModule(string id, Func<TView> viewBuilder, Func<TDataContext>? dataContextBuilder, NavigationModuleLifecycleOptions? lifecycleOptions) :
            base(id, viewBuilder, dataContextBuilder, lifecycleOptions)
        { }

        /// <inheritdoc/>
        protected override TDataContext? GetDataContext(TView view)
        {
            if (view.DataContext is not null && view.DataContext is not INavigationParticipant)
            {
                throw new ArgumentException($"Data context of provided parameter must be of type {typeof(INavigationParticipant)}", nameof(view));
            }

            return view.DataContext as TDataContext;
        }

        /// <inheritdoc/>
        protected override void SetDataContext(TView view, TDataContext? dataContext)
            => view.DataContext = dataContext;
    }
}
