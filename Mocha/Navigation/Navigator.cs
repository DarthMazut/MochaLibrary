using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mocha.Navigation
{
    /// <summary>
    /// Exposes API for navigation.
    /// </summary>
    public class Navigator
    {
        #region PRIVATE FIELDS

        private INavigationModule _hostView;
        private NavigationService _navigationService;

        #endregion

        #region EVENTS

        public delegate void NavigationRequestedEventHandler(NavigationData navigationData, NavigationCancelEventArgs e);
        public delegate void NavigationEventHandler(NavigationData navigationData);

        /// <summary>
        /// Fires whenever any <see cref="INavigatable"/> requested a navigation to <see cref="INavigationModule"/> that
        /// holds this <see cref="Navigator"/> instance. Do not put any set up code here including event subscribtion as 
        /// navigation can be rejected at this point. Use this event for deciding whether to reject navigation process.
        /// </summary>
        public event NavigationRequestedEventHandler NavigatingTo;

        /// <summary>
        /// Fires whenever <see cref="INavigationModule"/> that holds this <see cref="Navigator"/> instance is currently 
        /// active and the request has been made to navigate to antoher <see cref="INavigationModule"/> instance.
        /// Currently active <see cref="Navigator"/> can reject the navigation at this point. Any cleaning code 
        /// should be put here including unsubscribing from events.
        /// </summary>
        public event NavigationRequestedEventHandler NavigatingFrom;

        /// <summary>
        /// Fires when navigation process is about to finish and new view is displayed.
        /// This event should contain a set up code for parent <see cref="INavigatable"/>
        /// including event subscribtion. At this point navigation process cannot be rejected. 
        /// </summary>
        public event NavigationEventHandler NavigatedTo;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Determines whether <see cref="INavigationModule"/> associated with this
        /// <see cref="Navigator"/> object should be cached.
        /// </summary>
        public bool SaveCurrent { get; set; }

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Returns new instance of <see cref="Navigator"/> class.
        /// </summary>
        /// <param name="host">
        /// An object that hosts this instance by implementing <see cref="INavigatable"/> interface.
        /// Pass <see langword="this"/> here.
        /// </param>
        /// <param name="navigationService">
        /// A <see cref="NavigationService"/> object which will process 
        /// the requestes send by this instance.
        /// </param>
        public Navigator(INavigatable host, NavigationService navigationService)
        {
            if (host == null || navigationService == null) throw new ArgumentNullException();

            _hostView = new PassiveModule(host);
            _navigationService = navigationService;
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Sends navigation request to navigate to specified view.
        /// </summary>
        /// <param name="view">Navigation target view.</param>
        public Task<NavigationResultData> NavigateAsync(INavigationModule view)
        {
            return NavigateAsync(view, null, null, false);
        }

        /// <summary>
        /// Sends navigation request to navigate to specified view.
        /// </summary>
        /// <param name="view">Navigation target view.</param>
        /// <param name="data">
        /// An extra data object used to pass information between <see cref="INavigatable"/>
        /// objects that take part in navigation transition.
        /// </param>
        public Task<NavigationResultData> NavigateAsync(INavigationModule view, object data)
        {
            return NavigateAsync(view, data, null, false);
        }

        /// <summary>
        /// Sends navigation request to navigate to specified view.
        /// </summary>
        /// <param name="view">Navigation target view.</param>
        /// <param name="data">
        /// An extra data object used to pass information between <see cref="INavigatable"/>
        /// objects that take part in navigation transition.
        /// </param>
        /// <param name="navigatable">A * DataContext * object for target view.</param>
        public Task<NavigationResultData> NavigateAsync(INavigationModule view, object data, INavigatable navigatable)
        {
            return NavigateAsync(view, data, navigatable, false);
        }

        /// <summary>
        /// Sends navigation request to navigate to specified view.
        /// </summary>
        /// <param name="view">Navigation target view.</param>
        /// <param name="data">
        /// An extra data object used to pass information between <see cref="INavigatable"/>
        /// objects that take part in navigation transition.
        /// </param>
        /// <param name="navigatable">A * DataContext * object for target view.</param>
        /// <param name="ignoreCached">Determines whether cached <see cref="INavigationModule"/> 
        /// objects should be unconditionaly ingored for rising navigation process.</param>
        public Task<NavigationResultData> NavigateAsync(INavigationModule view, object data, INavigatable navigatable, bool ignoreCached)
        {
            if (view == null) throw new ArgumentNullException("Navigation target cannot be null");

            NavigationData navigationData = new NavigationData
            {
                CallingModule = _hostView,
                Data = data,
                IgnoreCached = ignoreCached,
                RequestedModule = view,
                SaveCurrent = _navigationService.CurrentView?.DataContext.Navigator.SaveCurrent ?? false
            };

            if(navigatable != null)
            {
                navigationData.RequestedModule.SetDataContext(navigatable);
            }

            return _navigationService.RequestNavigation(navigationData);
        }

        /// <summary>
        /// Sends navigation request to navigate with specified <see cref="NavigationData"/> object.
        /// </summary>
        /// <param name="navigationData">Contains details for requested navigation process.</param>
        public Task<NavigationResultData> Navigate(NavigationData navigationData)
        {
            if (navigationData == null) throw new ArgumentNullException();
            return _navigationService.RequestNavigation(navigationData);
        }

        #endregion

        #region INTERNAL METHODS

        /// <summary>
        /// Used internally by <see cref="NavigationService"/>.
        /// Prepares an <see cref="INavigatable"/> object for which navigation has been requested.
        /// </summary>
        /// <param name="navigationData">Details on navigation request.</param>
        /// <param name="e">Allow to reject navigation request.</param>
        internal void OnNavigatingToBase(NavigationData navigationData, NavigationCancelEventArgs e)
        {
            _hostView = navigationData.RequestedModule;
            NavigatingTo?.Invoke(navigationData, e);
        }

        /// <summary>
        /// Used internally by <see cref="NavigationService"/>.
        /// Prepares an <see cref="INavigatable"/> object for which navigation has been requested.
        /// </summary>
        /// <param name="navigationData">Details on navigation request.</param>
        internal void OnNavigatedToBase(NavigationData navigationData)
        {
            NavigatedTo?.Invoke(navigationData);
        }

        /// <summary>
        /// Used internally by <see cref="NavigationService"/>.
        /// Cleans up <see cref="INavigatable"/> object which was currently active.
        /// </summary>
        /// <param name="navigationData">Details on navigation request.</param>
        /// <param name="e">Allow to reject navigation request.</param>
        internal void OnNavigatingFromBase(NavigationData navigationData, NavigationCancelEventArgs e)
        {
            NavigatingFrom?.Invoke(navigationData, e);
        }

        #endregion

        #region PASSIVE MODULE

        private class PassiveModule : INavigationModule
        {
            public object View => null;

            public INavigatable DataContext { get; }

            public PassiveModule(INavigatable host)
            {
                DataContext = host;
            }

            public void CleanUp() { }

            public void SetDataContext(INavigatable dataContext) { }
        } 

        #endregion
    }
}
