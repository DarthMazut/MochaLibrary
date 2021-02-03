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
    /// Contains a core logic for navigation handling.
    /// </summary>
    public class NavigationService
    {
        private Dictionary<Type, INavigationModule> _cachedTypes = new Dictionary<Type, INavigationModule>();

        private INavigationModule _currentView;

        /// <summary>
        /// Returns a currently active <see cref="INavigationModule"/> object.
        /// </summary>
        public INavigationModule CurrentView => _currentView;

        /// <summary>
        /// Fires every time a navigation action has been requested by one of the <see cref="IINavigatable"/> object.
        /// </summary>
        public event EventHandler<NavigationData> NavigationRequested;

        /// <summary>
        /// Handles a navigation requests.
        /// </summary>
        /// <param name="navigationData">Essential data for navigation process.</param>
        public NavigationResultData RequestNavigation(NavigationData navigationData)
        {
            if (SameViewRequested(navigationData))
            {
                return new NavigationResultData(NavigationResult.SameModuleRequested);
            }

            ThrowIfCallerIsNull(navigationData);
            SetPreviousView(navigationData);

            bool cleanUp = HandleCache(navigationData);

            NavigationResultData resultTo = HandleNavigatingTo(navigationData);
            if (resultTo.Result != NavigationResult.Succeed)
            {
                return resultTo;
            }

            NavigationResultData resultFrom = HandleNavigatingFrom(navigationData);
            if (resultFrom.Result != NavigationResult.Succeed)
            {
                navigationData.RequestedModule.CleanUp();
                return resultFrom;
            }

            InvokeNavigatedRequested(navigationData);
            HandleNavigatedTo(navigationData);

            if (cleanUp)
            {
                CleanUp(navigationData);
            }

            return new NavigationResultData(NavigationResult.Succeed);
        }

        /// <summary>
        /// Clears every cached <see cref="INavigatable"/> objects.
        /// </summary>
        public void ClearCached()
        {
            _cachedTypes.Clear();
        }

        /// <summary>
        /// Removes cached <see cref="INavigatable"/> object of given type. 
        /// Returns <see langword="true"/> if specified type was found and removed, otherwise <see langword="false"/>.
        /// </summary>
        /// <param name="type">Type to be found and removed from cache.</param>
        public bool ClearCached(Type type)
        {
            if (type == null) return false;

            return _cachedTypes.Remove(type);
        }

        /// <summary>
        /// Checks whether given type it cached.
        /// </summary>
        /// <param name="type">Type to be checked.</param>
        public bool CheckCached(Type type)
        {
            return _cachedTypes.ContainsKey(type);
        }

        private bool HandleCache(NavigationData navigationData)
        {
            // CacheCurrent
            bool cleanUp = CacheCurrent(navigationData);

            // LoadCached
            Type requestedType = LoadCache(navigationData);

            // ClearCache
            ClearCached(requestedType);

            return cleanUp;
        }

        private Type LoadCache(NavigationData navigationData)
        {
            Type requestedType = navigationData.RequestedModule.DataContext.GetType();
            if (!navigationData.IgnoreCached)
            {
                if (_cachedTypes.ContainsKey(requestedType))
                {
                    if (_cachedTypes[requestedType].Equals(navigationData.RequestedModule))
                    {
                        navigationData.RequestedModule = _cachedTypes[requestedType];
                    }
                }
            }

            return requestedType;
        }

        private bool CacheCurrent(NavigationData navigationData)
        {
            bool cleanUp = true;

            if (navigationData.SaveCurrent)
            {
                Type searchedType = navigationData.PreviousModule.DataContext.GetType();
                if (_cachedTypes.ContainsKey(searchedType))
                {
                    _cachedTypes[searchedType] = navigationData.PreviousModule;
                }
                else
                {
                    _cachedTypes.Add(searchedType, navigationData.PreviousModule);
                }

                cleanUp = false;
            }

            return cleanUp;
        }

        private void SetPreviousView(NavigationData navigationData)
        {
            navigationData.PreviousModule = _currentView;
        }

        private static void ThrowIfCallerIsNull(NavigationData navigationData)
        {
            if (navigationData.CallingModule?.DataContext != null)
            {
                return;
            }
            else
            {
                throw new InvalidOperationException("Cannot navigate because caller is null.");
            }
        }

        private bool SameViewRequested(NavigationData navigationData)
        {
            return _currentView != null && _currentView.Equals(navigationData.RequestedModule);
        }

        private NavigationResultData HandleNavigatingTo(NavigationData navigationData)
        {
            NavigationCancelEventArgs e = new NavigationCancelEventArgs();
            navigationData.RequestedModule.DataContext.Navigator.OnNavigatingToBase(navigationData, e);
            if (e.Cancel)
            {
                navigationData.RequestedModule.CleanUp();
                return new NavigationResultData(NavigationResult.RejectedByTarget, e.Reason);
            }

            return new NavigationResultData(NavigationResult.Succeed);
        }

        private NavigationResultData HandleNavigatingFrom(NavigationData navigationData)
        {
            NavigationCancelEventArgs e = new NavigationCancelEventArgs();
            navigationData.CallingModule?.DataContext.Navigator.OnNavigatingFromBase(navigationData, e);
            if (e.Cancel)
            {
                navigationData.RequestedModule.CleanUp();
                return new NavigationResultData(NavigationResult.RejectedByCurrent, e.Reason);
            }

            return new NavigationResultData(NavigationResult.Succeed);
        }

        private static void HandleNavigatedTo(NavigationData navigationData)
        {
            navigationData.RequestedModule.DataContext.Navigator.OnNavigatedToBase(navigationData);
        }

        private void InvokeNavigatedRequested(NavigationData navigationData)
        {
            NavigationRequested?.Invoke(this, navigationData);
            _currentView = navigationData.RequestedModule;
        }

        private void CleanUp(NavigationData navigationData)
        {
            navigationData.PreviousModule?.CleanUp();
        }
    }
}
