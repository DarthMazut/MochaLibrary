using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public interface INavigationService
    {
        /// <summary>
        /// Determines whether this <see cref="INavigationService"/> is initialized.
        /// </summary>
        public bool IsInitialized { get; }

        /// <summary>
        /// Returns currently active <see cref="INavigationModule"/>.
        /// </summary>
        public INavigationModule CurrentModule { get; }

        /// <summary>
        /// Returns currently active <see cref="INavigationStackItem"/>.
        /// </summary>
        public INavigationStackItem CurrentItem { get; }

        /// <summary>
        /// Returns a dictionary of all registered <see cref="INavigationModule"/> with its identifier
        /// as a key.
        /// </summary>
        public IReadOnlyDictionary<string, INavigationModule> AvailableModules { get; }

        /// <summary>
        /// Returns current navigation stack as a readonly collection.
        /// </summary>
        public IReadOnlyNavigationStack<INavigationStackItem> NavigationHistory { get; }

        /// <summary>
        /// Occurs when <see cref="CurrentModule"/> changes its value.
        /// </summary>
        public event EventHandler<CurrentNavigationModuleChangedEventArgs> CurrentModuleChanged;

        /// <summary>
        /// Handles a navigation requests.
        /// </summary>
        /// <param name="requestData">Essential data for navigation process.</param>
        public Task<NavigationResultData> RequestNavigation(NavigationRequestData requestData);

        /// <summary>
        /// Initializes an <see cref="INavigationService"/> by setting initialized initial module as <see cref="CurrentModule"/>.
        /// </summary>
        public Task Initialize();

        /// <summary>
        /// Initializes an <see cref="INavigationService"/> by setting initialized initial module as <see cref="CurrentModule"/>.
        /// </summary>
        /// <param name="callSubscribersOnInit">
        /// If set to <see langword="true"/> the <see cref="IOnNavigatedTo"/> and
        /// <see cref="IOnNavigatedToAsync"/> methods will be called on the initial module.
        /// </param>
        public Task Initialize(bool callSubscribersOnInit);

        /// <summary>
        /// Uninitializes <see cref="INavigationService"/> by uninitializing <see cref="INavigationModule"/>
        /// which is on top of navigation stack. This method considers 
        /// <see cref="NavigationModuleLifecycleOptions.PreferCache"/> and <see cref="Navigator.SaveCurrent"/> properties.
        /// </summary>
        public void Uninitialize();

        /// <summary>
        /// Uninitializes <see cref="INavigationService"/> by uninitializing <see cref="INavigationModule"/>
        /// which is on top of navigation stack. This method considers 
        /// <see cref="NavigationModuleLifecycleOptions.PreferCache"/> and <see cref="Navigator.SaveCurrent"/> properties.
        /// </summary>
        /// <param name="clearStack">
        /// Determines whether entire navigation stack should be cleared. If this value is set to
        /// <see langword="true"/> <see cref="NavigationModuleLifecycleOptions.PreferCache"/> and
        /// <see cref="Navigator.SaveCurrent"/> properties are ignored.
        /// </param>
        public void Uninitialize(bool clearStack);
    }
}
