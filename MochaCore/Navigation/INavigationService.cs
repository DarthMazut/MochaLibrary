using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Provides state and logic for single navigatable area. Each implementation of <see cref="INavigationService"/>
    /// manages its own navigation stack and handles lifecycle of registered <see cref="INavigationModule"/>
    /// objects. Using the service you're defining custom navigation behaviour (what navigation means to your 
    /// application; how should be handled) specific to your application needs by subscribing to 
    /// <see cref="CurrentModuleChanged"/>. Navigation process is invoked by calling 
    /// <see cref="RequestNavigation(NavigationRequestData)"/> method.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Determines whether this <see cref="INavigationService"/> is initialized.
        /// <see cref="INavigationService"/> is initialized when current <see cref="INavigationModule"/>
        /// is initialized.
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
        /// Handles navigation requests of type <see cref="NavigationType.PushModal"/>.
        /// Returns data provided by the navigation target's call to <see cref="INavigator.ReturnModal(object)"/>.
        /// Throws if the specified <see cref="NavigationType"/> is not modal.
        /// </summary>
        /// <param name="requestData">Essential data for the navigation process.</param>
        public Task<object?> RequestModalNavigation(NavigationRequestData requestData);

        /// <summary>
        /// Handles a navigation requests.
        /// </summary>
        /// <param name="requestData">Essential data for navigation process.</param>
        public Task<NavigationResultData> RequestNavigation(NavigationRequestData requestData);

        /// <summary>
        /// Initializes an <see cref="INavigationService"/> by initializing the current <see cref="INavigationModule"/>.
        /// If the <see cref="CurrentModule"/> is not determined at the time of calling, it will be set to the initial module.
        /// </summary>
        public Task Initialize();

        /// <summary>
        /// Initializes an <see cref="INavigationService"/> by initializing the current <see cref="INavigationModule"/>.
        /// If the <see cref="CurrentModule"/> is not determined at the time of calling, it will be set to the initial module.
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
        /// <br/>
        /// You should call this method to uninitialize <see cref="INavigationService"/> instances that are currently unreachable 
        /// (e.g., within a closed window or a page that is not currently displayed).
        /// </summary>
        public void Uninitialize();

        /// <summary>
        /// Uninitializes <see cref="INavigationService"/> by uninitializing <see cref="INavigationModule"/>
        /// which is on top of navigation stack. This method considers 
        /// <see cref="NavigationModuleLifecycleOptions.PreferCache"/> and <see cref="Navigator.SaveCurrent"/> properties.
        /// You should call this method to uninitialize <see cref="INavigationService"/> instances that are currently unreachable 
        /// (e.g., within a closed window or a page that is not currently displayed).
        /// </summary>
        /// <param name="clearStack">
        /// Determines whether entire navigation stack should be cleared. If this value is set to
        /// <see langword="true"/> <see cref="NavigationModuleLifecycleOptions.PreferCache"/> and
        /// <see cref="Navigator.SaveCurrent"/> properties are ignored.
        /// </param>
        public void Uninitialize(bool clearStack);
    }
}
