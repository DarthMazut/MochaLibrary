using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Extends <see cref="INavigationModule"/> with 
    /// </summary>
    public interface INavigationLifecycleModule : INavigationModule
    {
        public NavigationModuleLifecycleOptions LifecycleOptions { get; }

        public void Initialize();

        public void Initialize(object view);

        public void Initialize(object view, INavigatable dataContext);

        public void Uninitialize();
    }
}
