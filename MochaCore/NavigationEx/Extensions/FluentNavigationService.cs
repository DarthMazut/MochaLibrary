using System;

namespace MochaCore.NavigationEx.Extensions
{
    /// <summary>
    /// Extends <see cref="BaseNavigationService"/> with fluent API.
    /// </summary>
    /// <typeparam name="T">The most basic type of implementing technology that supports <i>DataContext</i> assignment.</typeparam>
    public abstract class FluentNavigationService<T> : BaseNavigationService where T : class
    {
        public FluentNavigationService<T> WithModule<TView, TDataContext>() 
            where TView : T 
            where TDataContext : class, INavigatable
        {
            AddModule(CreateModule<TView, TDataContext>(nameof(TView), () => Activator.CreateInstance<TView>(), null, null));
            return this;
        }

        public FluentNavigationService<T> WithModule<TView, TDataContext>(string id)
            where TView : T
            where TDataContext : class, INavigatable
        {
            AddModule(CreateModule<TView, TDataContext>(id, () => Activator.CreateInstance<TView>(), null, null));
            return this;
        }

        public FluentNavigationService<T> WithModule<TView, TDataContext>(string id, NavigationModuleLifecycleOptions lifecycleOptions)
            where TView : T
            where TDataContext : class, INavigatable
        {
            AddModule(CreateModule<TView, TDataContext>(id, () => Activator.CreateInstance<TView>(), null, lifecycleOptions));
            return this;
        }

        public FluentNavigationService<T> WithModule<TView, TDataContext>(string id, Func<TView> viewBuilder, Func<TDataContext> dataContextBuilder)
            where TView : T
            where TDataContext : class, INavigatable
        {
            AddModule(CreateModule(id, viewBuilder, dataContextBuilder, null));
            return this;
        }

        public FluentNavigationService<T> WithModule<TView, TDataContext>(string id, Func<TView> viewBuilder, Func<TDataContext> dataContextBuilder, NavigationModuleLifecycleOptions lifecycleOptions)
            where TView : T
            where TDataContext : class, INavigatable
        {
            AddModule(CreateModule(id, viewBuilder, dataContextBuilder, lifecycleOptions));
            return this;
        }

        /// <summary>
        /// Sets identifier of default <see cref="INavigationModule"/>.
        /// </summary>
        /// <param name="id">Id of <see cref="INavigationModule"/> to be treated as default one.</param>
        public FluentNavigationService<T> WithInitialId(string id)
        {
            SelectInitialtId(id);
            return this;
        }

        protected abstract INavigationLifecycleModule CreateModule<TView, TDataContext>(
            string id,
            Func<TView> viewBuilder,
            Func<TDataContext>? dataContextBuilder,
            NavigationModuleLifecycleOptions? lifecycleOptions)
        where TView : T
        where TDataContext : class, INavigatable;
    }
}
