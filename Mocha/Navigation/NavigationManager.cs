using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Navigation
{
    /// <summary>
    /// Mediates between the technology-specific and technology-independent layers.
    /// </summary>
    public static class NavigationManager
    {
        private static readonly Dictionary<string, Func<INavigationModule>> _dictionary = new Dictionary<string, Func<INavigationModule>>();

        /// <summary>
        /// Stores the function that creates the <see cref="INavigationModule"/> which can be used later
        /// by the technology-independent layer.
        /// </summary>
        /// <param name="id">
        /// The identifier by which you can find the delegate creating a specific 
        /// <see cref="INavigationModule"/> from the technology-independent layer.
        /// </param>
        /// <param name="navigationViewDelegate">A delegate which creates a <see cref="INavigationModule"/> object.</param>
        public static void AddModule(string id, Func<INavigationModule> navigationViewDelegate)
        {
            if(!_dictionary.ContainsKey(id))
            {
                _dictionary.Add(id, navigationViewDelegate);
            }
            else
            {
                throw new InvalidOperationException($"Cannot add INavigationModule with id {id} because it was already registered.");
            }
            
        }

        /// <summary>
        /// Returns a new instance of <see cref="INavigationModule"/> object associated
        /// with given identifier.
        /// </summary>
        /// <param name="id">Identifier for <see cref="INavigationModule"/> object to be retrieved.</param>
        public static INavigationModule FetchModule(string id)
        {
            if(_dictionary.ContainsKey(id))
            {
                return _dictionary[id]?.Invoke();
            }
            else
            {
                throw new ArgumentException($"Cannot fetch module {id} because it hasn't been registerd. Ensure ID was correctly spelled.");
            }
            
        }
    }
}
