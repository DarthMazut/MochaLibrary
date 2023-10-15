using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Navigation.Extensions.DI
{
    /// <summary>
    /// Provides implementation of <see cref="INavigationServiceProvider"/>.
    /// </summary>
    public class NavigationServiceProvider : INavigationServiceProvider
    {
        /// <inheritdoc/>
        public INavigationService FetchNavigationService(string id) => NavigationManager.FetchNavigationService(id);
    }
}
