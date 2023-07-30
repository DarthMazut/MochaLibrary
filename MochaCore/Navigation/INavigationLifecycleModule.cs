using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Extends <see cref="INavigationModule"/> with lifecycle API.
    /// </summary>
    public interface INavigationLifecycleModule : INavigationModule
    {
        /// <summary>
        /// Provides additional settings for managing lifecycle of <see cref="INavigationLifecycleModule"/>.
        /// </summary>
        public NavigationModuleLifecycleOptions LifecycleOptions { get; }

        /// <summary>
        /// Initializes the current <see cref="INavigationLifecycleModule"/> by setting the values
        /// of the <see cref="INavigationModule.View"/> and the <see cref="INavigationModule.DataContext"/> properties.
        /// </summary>
        public void Initialize();

        /// <summary>
        /// Uninitializes the current <see cref="INavigationLifecycleModule"/> by setting the <see cref="INavigationModule.View"/> and
        /// <see cref="INavigationModule.DataContext"/> properties to <see langword="null"/>. Ensures that the DataContext on the View object is also nullified.
        /// If the <see cref="INavigationModule.DataContext"/> is an instance of <see cref="IDisposable"/> and the related <see cref="NavigationModuleLifecycleOptions.DisposeDataContextOnUninitialize"/>
        /// property is set to <see langword="true"/>, it calls the Dispose method on the DataContext.
        /// </summary>
        public void Uninitialize();
    }
}
