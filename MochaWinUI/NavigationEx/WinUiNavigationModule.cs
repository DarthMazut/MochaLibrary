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
    public sealed class WinUiNavigationModule : BaseNavigationModule<FrameworkElement>
    {
        private WinUiNavigationModule(
            string id,
            Func<FrameworkElement> viewBuilder,
            Type viewType,
            Func<INavigatable>? dataContextBuilder,
            Type? dataContextType,
            NavigationModuleLifecycleOptions? lifecycleOptions)
            : base(id, viewBuilder, viewType, dataContextBuilder, dataContextType, lifecycleOptions) { }

        /// <summary>
        /// Initializes new instance of the <see cref="WinUiNavigationModule"/> class.
        /// </summary>
        /// <typeparam name="TView">
        /// Type of <see cref="INavigationModule.View"/> object associated with creating instance.
        /// </typeparam>
        /// <typeparam name="TDataContext">
        /// Type of <see cref="INavigationModule.DataContext"/> object associated with creating instance.
        /// </typeparam>
        public static WinUiNavigationModule Create<TView, TDataContext>()
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
            => new WinUiNavigationModule(
                nameof(TView),
                () => Activator.CreateInstance<TView>(),
                typeof(TView),
                null,
                typeof(TDataContext),
                null);

        /// <summary>
        /// Initializes new instance of the <see cref="WinUiNavigationModule"/> class.
        /// </summary>
        /// <typeparam name="TView">
        /// Type of <see cref="INavigationModule.View"/> object associated with creating instance.
        /// </typeparam>
        /// <typeparam name="TDataContext">
        /// Type of <see cref="INavigationModule.DataContext"/> object associated with creating instance.
        /// </typeparam>
        /// <param name="id">Identifier of creating module.</param>
        public static WinUiNavigationModule Create<TView, TDataContext>(string id)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
            => new WinUiNavigationModule(
                id,
                () => Activator.CreateInstance<TView>(),
                typeof(TView),
                null,
                typeof(TDataContext),
                null);

        /// <summary>
        /// Initializes new instance of the <see cref="WinUiNavigationModule"/> class.
        /// </summary>
        /// <typeparam name="TView">
        /// Type of <see cref="INavigationModule.View"/> object associated with creating instance.
        /// </typeparam>
        /// <typeparam name="TDataContext">
        /// Type of <see cref="INavigationModule.DataContext"/> object associated with creating instance.
        /// </typeparam>
        /// <param name="lifecycleOptions">
        /// Provides additional settings for managing lifecycle of <see cref="INavigationLifecycleModule"/>.
        /// </param>
        public static WinUiNavigationModule Create<TView, TDataContext>(NavigationModuleLifecycleOptions lifecycleOptions)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
            => new WinUiNavigationModule(
                nameof(TView),
                () => Activator.CreateInstance<TView>(),
                typeof(TView),
                null,
                typeof(TDataContext),
                lifecycleOptions);


        /// <summary>
        /// Initializes new instance of the <see cref="WinUiNavigationModule"/> class.
        /// </summary>
        /// <typeparam name="TView">
        /// Type of <see cref="INavigationModule.View"/> object associated with creating instance.
        /// </typeparam>
        /// <typeparam name="TDataContext">
        /// Type of <see cref="INavigationModule.DataContext"/> object associated with creating instance.
        /// </typeparam>
        /// <param name="id">Identifier of creating module.</param>
        /// <param name="lifecycleOptions">
        /// Provides additional settings for managing lifecycle of <see cref="INavigationLifecycleModule"/>.
        /// </param>
        public static WinUiNavigationModule Create<TView, TDataContext>(string id, NavigationModuleLifecycleOptions lifecycleOptions)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
            => new WinUiNavigationModule(
                id,
                () => Activator.CreateInstance<TView>(),
                typeof(TView),
                null,
                typeof(TDataContext),
                lifecycleOptions);

        /// <summary>
        /// Initializes new instance of the <see cref="WinUiNavigationModule"/> class.
        /// </summary>
        /// <typeparam name="TView">
        /// Type of <see cref="INavigationModule.View"/> object associated with creating instance.
        /// </typeparam>
        /// <typeparam name="TDataContext">
        /// Type of <see cref="INavigationModule.DataContext"/> object associated with creating instance.
        /// </typeparam>
        /// <param name="id">Identifier of creating module.</param>
        /// <param name="viewBuilder">
        /// A delegate that returns new instance of *TView* class.
        /// This will be invoked when creating instance receives call to its <see cref="INavigationLifecycleModule.Initialize"/> implementation.
        /// </param>
        /// <param name="dataContextBuilder">
        /// A delegate that returns new instance of *TDataContext* class.
        /// This will be invoked when creating instance receives call to its <see cref="INavigationLifecycleModule.Initialize"/> implementation.
        /// </param>
        public static WinUiNavigationModule Create<TView, TDataContext>(string id, Func<TView> viewBuilder, Func<TDataContext> dataContextBuilder) 
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
            => new WinUiNavigationModule(id, viewBuilder, typeof(TView), dataContextBuilder, typeof(TDataContext), null);

        /// <summary>
        /// Initializes new instance of the <see cref="WinUiNavigationModule"/> class.
        /// </summary>
        /// <typeparam name="TView">
        /// Type of <see cref="INavigationModule.View"/> object associated with creating instance.
        /// </typeparam>
        /// <typeparam name="TDataContext">
        /// Type of <see cref="INavigationModule.DataContext"/> object associated with creating instance.
        /// </typeparam>
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
        public static WinUiNavigationModule Create<TView, TDataContext>(
            string id,
            Func<TView> viewBuilder,
            Func<TDataContext> dataContextBuilder,
            NavigationModuleLifecycleOptions lifecycleOptions)
                where TView : FrameworkElement
                where TDataContext : class, INavigatable
                    => new WinUiNavigationModule(
                        id,
                        viewBuilder,
                        typeof(TView),
                        dataContextBuilder,
                        typeof(TDataContext),
                        lifecycleOptions);

        /// <inheritdoc/>
        protected override INavigatable? GetDataContext(FrameworkElement view)
        {
            if (view.DataContext is not null && view.DataContext is not INavigatable)
            {
                throw new ArgumentException($"Data context of provided parameter must be of type {typeof(INavigatable)}", nameof(view));
            }

            return view.DataContext as INavigatable;
        }

        /// <inheritdoc/>
        protected override void SetDataContext(FrameworkElement view, INavigatable? dataContext)
            => view.DataContext = dataContext;
    }
}
