using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Navigation.Extensions.DI
{
    /// <summary>
    /// Allows for retrieving <see cref="INavigationService"/> by its identifier.
    /// Use this interface when choosing DI approach.
    /// </summary>
    public interface INavigationServiceProvider
    {
        /// <summary>
        /// Retrieves registered <see cref="INavigationService"/> instance by its identifier.
        /// </summary>
        /// <param name="id"><see cref="INavigationService"/> unique identifier.</param>
        /// <exception cref="ArgumentException"></exception>
        public INavigationService FetchNavigationService(string id);
    }
}
