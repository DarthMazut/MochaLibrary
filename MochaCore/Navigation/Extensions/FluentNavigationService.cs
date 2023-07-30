using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx.Extensions
{
    /// <summary>
    /// Extends <see cref="NavigationService"/> with fluent API.
    /// </summary>
    /// <typeparam name="T">The most basic type of implementing technology that supports <i>DataContext</i> assignment.</typeparam>
    public abstract class FluentNavigationService<T> : NavigationService where T : class
    {
        /// <summary>
        /// Registers <see cref="INavigationModule"/> implementation for current service.
        /// </summary>
        /// <remarks>
        /// By default the identifier of registering module will be the same as its type name, 
        /// unless explicitly specified otherwise. <br/>
        /// It's possible to omit explicit declaration of construction delegates if the creating 
        /// types have parameterless constructors. 
        /// </remarks>
        /// <typeparam name="TView">View type of registering module.</typeparam>
        /// <typeparam name="TDataContext">Data context type of registering module.</typeparam>
        public FluentNavigationService<T> WithModule<TView, TDataContext>()
            where TView : T
            where TDataContext : class, INavigationParticipant
        {
            AddModule(CreateModule<TView, TDataContext>(typeof(TView).Name, () => Activator.CreateInstance<TView>(), null, null));
            return this;
        }

        /// <summary>
        /// Registers <see cref="INavigationModule"/> implementation for current service.
        /// </summary>
        /// <remarks>
        /// It's possible to omit explicit declaration of construction delegates if the creating 
        /// types have parameterless constructors. 
        /// </remarks>
        /// <typeparam name="TView">View type of registering module.</typeparam>
        /// <typeparam name="TDataContext">Data context type of registering module.</typeparam>
        /// <param name="id">Identifier of registering module.</param>
        public FluentNavigationService<T> WithModule<TView, TDataContext>(string id)
            where TView : T
            where TDataContext : class, INavigationParticipant
        {
            AddModule(CreateModule<TView, TDataContext>(id, () => Activator.CreateInstance<TView>(), null, null));
            return this;
        }

        /// <summary>
        /// Registers <see cref="INavigationModule"/> implementation for current service.
        /// </summary>
        /// <remarks>
        /// By default the identifier of registering module will be the same as its type name, 
        /// unless explicitly specified otherwise. <br/>
        /// It's possible to omit explicit declaration of construction delegates if the creating 
        /// types have parameterless constructors. 
        /// </remarks>
        /// <typeparam name="TView">View type of registering module.</typeparam>
        /// <typeparam name="TDataContext">Data context type of registering module.</typeparam>
        /// <param name="lifecycleOptions">
        /// Provides additional settings for managing lifecycle of <see cref="INavigationLifecycleModule"/>.
        /// </param>
        public FluentNavigationService<T> WithModule<TView, TDataContext>(NavigationModuleLifecycleOptions lifecycleOptions)
            where TView : T
            where TDataContext : class, INavigationParticipant
        {
            AddModule(CreateModule<TView, TDataContext>(typeof(TView).Name, () => Activator.CreateInstance<TView>(), null, lifecycleOptions));
            return this;
        }

        /// <summary>
        /// Registers <see cref="INavigationModule"/> implementation for current service.
        /// </summary>
        /// <remarks>
        /// It's possible to omit explicit declaration of construction delegates if the creating 
        /// types have parameterless constructors. 
        /// </remarks>
        /// <typeparam name="TView">View type of registering module.</typeparam>
        /// <typeparam name="TDataContext">Data context type of registering module.</typeparam>
        /// <param name="id">Identifier of registering module.</param>
        /// <param name="lifecycleOptions">
        /// Provides additional settings for managing lifecycle of <see cref="INavigationLifecycleModule"/>.
        /// </param>
        public FluentNavigationService<T> WithModule<TView, TDataContext>(string id, NavigationModuleLifecycleOptions lifecycleOptions)
            where TView : T
            where TDataContext : class, INavigationParticipant
        {
            AddModule(CreateModule<TView, TDataContext>(id, () => Activator.CreateInstance<TView>(), null, lifecycleOptions));
            return this;
        }

        /// <summary>
        /// Registers <see cref="INavigationModule"/> implementation for current service.
        /// </summary>
        /// <remarks>
        /// By default the identifier of registering module will be the same as its type name, 
        /// unless explicitly specified otherwise. <br/>
        /// </remarks>
        /// <typeparam name="TView">View type of registering module.</typeparam>
        /// <typeparam name="TDataContext">Data context type of registering module.</typeparam>
        /// <param name="viewBuilder">A delegate that returns a new instance of the view object associated with the creating module.</param>
        /// <param name="dataContextBuilder">
        /// A delegate that returns a new instance of the data context object associated with the creating module.
        /// </param>
        public FluentNavigationService<T> WithModule<TView, TDataContext>(Func<TView> viewBuilder, Func<TDataContext> dataContextBuilder)
            where TView : T
            where TDataContext : class, INavigationParticipant
        {
            AddModule(CreateModule(nameof(TView), viewBuilder, dataContextBuilder, null));
            return this;
        }

        /// <summary>
        /// Registers <see cref="INavigationModule"/> implementation for current service.
        /// </summary>
        /// <typeparam name="TView">View type of registering module.</typeparam>
        /// <typeparam name="TDataContext">Data context type of registering module.</typeparam>
        /// <param name="viewBuilder">A delegate that returns a new instance of the view object associated with the creating module.</param>
        /// <param name="dataContextBuilder">
        /// A delegate that returns a new instance of the data context object associated with the creating module.
        /// </param>
        /// <param name="id">Identifier of registering module.</param>
        public FluentNavigationService<T> WithModule<TView, TDataContext>(string id, Func<TView> viewBuilder, Func<TDataContext> dataContextBuilder)
            where TView : T
            where TDataContext : class, INavigationParticipant
        {
            AddModule(CreateModule(id, viewBuilder, dataContextBuilder, null));
            return this;
        }

        /// <summary>
        /// Registers <see cref="INavigationModule"/> implementation for current service.
        /// </summary>
        /// <typeparam name="TView">View type of registering module.</typeparam>
        /// <typeparam name="TDataContext">Data context type of registering module.</typeparam>
        /// <param name="viewBuilder">A delegate that returns a new instance of the view object associated with the creating module.</param>
        /// <param name="dataContextBuilder">
        /// A delegate that returns a new instance of the data context object associated with the creating module.
        /// </param>
        /// <param name="id">Identifier of registering module.</param>
        /// <param name="lifecycleOptions">
        /// Provides additional settings for managing lifecycle of <see cref="INavigationLifecycleModule"/>.
        /// </param>
        public FluentNavigationService<T> WithModule<TView, TDataContext>(string id, Func<TView> viewBuilder, Func<TDataContext> dataContextBuilder, NavigationModuleLifecycleOptions lifecycleOptions)
            where TView : T
            where TDataContext : class, INavigationParticipant
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

        /// <summary>
        /// Creates instance of technology-specific <see cref="INavigationLifecycleModule"/> implementation.
        /// </summary>
        /// <typeparam name="TView">View type of registering module.</typeparam>
        /// <typeparam name="TDataContext">Data context type of registering module.</typeparam>
        /// <param name="id">Identifier of registering module.</param>
        /// <param name="viewBuilder">A delegate that returns a new instance of the view object associated with the creating module.</param>
        /// <param name="dataContextBuilder">
        /// A delegate that returns a new instance of the data context object associated with the creating module.
        /// </param>
        /// <param name="lifecycleOptions">
        /// Provides additional settings for managing lifecycle of <see cref="INavigationLifecycleModule"/>.
        /// </param>
        protected abstract INavigationLifecycleModule CreateModule<TView, TDataContext>(
            string id,
            Func<TView> viewBuilder,
            Func<TDataContext>? dataContextBuilder,
            NavigationModuleLifecycleOptions? lifecycleOptions)
        where TView : T
        where TDataContext : class, INavigationParticipant;
    }
}
