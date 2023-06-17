using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public interface INavigatorInitialize
    {
        /// <summary>
        /// Called internally by <see cref="INavigationService"/> implementation
        /// to initialize <see cref="Navigator"/> object.
        /// </summary>
        /// <param name="module"><see cref="INavigationModule"/> associated with initializing <see cref="INavigator"/> instance.</param>
        /// <param name="navigationService"><see cref="INavigationService"/> associated with initializing <see cref="INavigator"/> instance.</param>
        public void Initialize(INavigationModule module, INavigationService navigationService);
    }
}
