using System;
using System.Threading.Tasks;

namespace MochaCore.Navigation
{
    /// <summary>
    /// Exposes API for navigation.
    /// </summary>
    public class Navigator
    {
        #region PRIVATE FIELDS

        private INavigationModule _hostView;
        private readonly INavigationService _navigationService;

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
        /// Initializes a new instance of the <see cref="Navigator"/> class.
        /// </summary>
        /// <param name="host">
        /// An object that hosts this instance by implementing <see cref="INavigatable"/> interface.
        /// Pass <see langword="this"/> here.
        /// </param>
        /// <param name="navigationService">
        /// A <see cref="NavigationService"/> object which will process 
        /// the requestes send by this instance.
        /// </param>
        public Navigator(INavigatable host, INavigationService navigationService)
        {
            _ = host ?? throw new ArgumentNullException(nameof(host));
            _ = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

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
        public Task<NavigationResultData> NavigateAsync(INavigationModule view, object? data, INavigatable? navigatable, bool ignoreCached)
        {
            _ = view ?? throw new ArgumentNullException(nameof(view), "Navigation target cannot be null");

            NavigationData navigationData = new()
            {
                CallingModule = _hostView,
                Data = data,
                IgnoreCached = ignoreCached,
                RequestedModule = view,
                SaveCurrent = _navigationService.CurrentView?.DataContext.Navigator.SaveCurrent ?? false
            };

            if (navigatable != null)
            {
                navigationData.RequestedModule.SetDataContext(navigatable);
            }

            return _navigationService.RequestNavigation(navigationData);
        }

        /// <summary>
        /// Sends navigation request to navigate with specified <see cref="NavigationData"/> object.
        /// </summary>
        /// <param name="navigationData">Contains details for requested navigation process.</param>
        public Task<NavigationResultData> NavigateAsync(NavigationData navigationData)
        {
            _ = navigationData ?? throw new ArgumentNullException(nameof(navigationData));

            return _navigationService.RequestNavigation(navigationData);
        }

        #endregion

        #region INTERNAL METHODS

        /// <summary>
        /// Associates <see cref="INavigationModule"/> with this instance. This cannot be done
        /// while defining <see cref="INavigationModule"/> because you can navigate with diffrents
        /// VM. VM does not hold a reference to parent <see cref="INavigationModule"/> so this have to be done
        /// after navigation requested.
        /// </summary>
        internal void SetHostView(NavigationData navigationData)
        {
            _hostView = navigationData.RequestedModule;
        }

        #endregion

        #region PASSIVE MODULE

        private class PassiveModule : INavigationModule
        {
            public object? View => null;

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
