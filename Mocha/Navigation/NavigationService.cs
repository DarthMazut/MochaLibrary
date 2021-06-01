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
    public class NavigationService : INavigationService
    {
        private List<INavigationModule> _cachedModules = new List<INavigationModule>();
        private INavigationModule _currentView;
        private NavigationModuleInternalComparer _moduleComparer = new NavigationModuleInternalComparer();

        /// <inheritdoc/>
        public INavigationModule CurrentView => _currentView;

        /// <inheritdoc/>
        public event EventHandler<NavigationData> NavigationRequested;

        /// <inheritdoc/>
        public async Task<NavigationResultData> RequestNavigation(NavigationData navigationData)
        {
            if (SameViewRequested(navigationData))
            {
                return new NavigationResultData(NavigationResult.SameModuleRequested);
            }

            ThrowIfCallerOrRequestedViewIsNull(navigationData);
            SetPreviousView(navigationData);

            bool cleanUp = HandleCache(navigationData);

            NavigationResultData resultTo = await HandleNavigatingTo(navigationData);
            if (resultTo.Result != NavigationResult.Succeed)
            {
                navigationData.RequestedModule.CleanUp();
                return resultTo;
            }

            NavigationResultData resultFrom = await HandleNavigatingFrom(navigationData);
            if (resultFrom.Result != NavigationResult.Succeed)
            {
                navigationData.RequestedModule.CleanUp();
                return resultFrom;
            }

            InvokeNavigatedRequested(navigationData);
            await HandleNavigatedTo(navigationData);

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
            _cachedModules.Clear();
        }

        /// <inheritdoc/>
        public bool ClearCached(INavigationModule module)
        {
            if (module == null) return false;

            return _cachedModules.RemoveAll(m =>
            {
                if (_moduleComparer.Equals(m, module))
                {
                    if (m != module)
                    {
                        m.CleanUp();
                    }
                    return true;
                }

                return false;
            }) > 0;
        }

        /// <inheritdoc/>
        public bool CheckCached(INavigationModule module)
        {
            return _cachedModules.Contains(module);
        }

        private bool HandleCache(NavigationData navigationData)
        {
            bool cleanUp = CacheCurrent(navigationData);

            LoadCache(navigationData);

            ClearCached(navigationData.RequestedModule);

            return cleanUp;
        }

        private void LoadCache(NavigationData navigationData)
        {
            if (!navigationData.IgnoreCached)
            {
                INavigationModule requestedModule = navigationData.RequestedModule;

                if (_cachedModules.Contains(requestedModule, _moduleComparer))
                {
                    navigationData.RequestedModule = _cachedModules.First(m => _moduleComparer.Equals(m, requestedModule));
                }
            }
        }

        private bool CacheCurrent(NavigationData navigationData)
        {
            bool cleanUp = true;

            if (navigationData.SaveCurrent)
            {
                ClearCached(navigationData.PreviousModule);
                _cachedModules.Add(navigationData.PreviousModule);

                cleanUp = false;
            }

            return cleanUp;
        }

        private void SetPreviousView(NavigationData navigationData)
        {
            navigationData.PreviousModule = _currentView;
        }

        private static void ThrowIfCallerOrRequestedViewIsNull(NavigationData navigationData)
        {
            if (navigationData.CallingModule?.DataContext == null)
            {
                throw new InvalidOperationException("Cannot navigate because caller is null.");
            }

            if (navigationData.RequestedModule.View == null)
            {
                throw new InvalidOperationException("Cannot navigate because requested module's view is null.");
            }
        }

        private bool SameViewRequested(NavigationData navigationData)
        {
            return _currentView != null && _moduleComparer.Equals(_currentView, navigationData.RequestedModule);
        }

        private async Task<NavigationResultData> HandleNavigatingTo(NavigationData navigationData)
        {
            navigationData.RequestedModule.DataContext.Navigator.SetHostView(navigationData);
            NavigationCancelEventArgs e = new NavigationCancelEventArgs();

            if (navigationData.RequestedModule.DataContext is IOnNavigatingTo onNavigatingTo)
            {
                onNavigatingTo.OnNavigatingTo(navigationData, e);
            }

            if (navigationData.RequestedModule.DataContext is IOnNavigatingToAsync onNavigatingToAsync)
            {
                await onNavigatingToAsync.OnNavigatingToAsync(navigationData, e);
            }

            if (e.Cancel)
            {
                navigationData.RequestedModule.CleanUp();
                return new NavigationResultData(NavigationResult.RejectedByTarget, e.Reason);
            }

            return new NavigationResultData(NavigationResult.Succeed);
        }

        private async Task<NavigationResultData> HandleNavigatingFrom(NavigationData navigationData)
        {
            // navigationData.RequestedModule.DataContext.Navigator.SetHostView(navigationData);
            NavigationCancelEventArgs e = new NavigationCancelEventArgs();

            if (navigationData.PreviousModule?.DataContext is IOnNavigatingFrom onNavigatingFrom)
            {
                onNavigatingFrom.OnNavigatingFrom(navigationData, e);
            }

            if (navigationData.PreviousModule?.DataContext is IOnNavigatingFromAsync onNavigatingFromAsync)
            {
                await onNavigatingFromAsync.OnNavigatingFromAsync(navigationData, e);
            }

            if (e.Cancel)
            {
                navigationData.RequestedModule.CleanUp();
                return new NavigationResultData(NavigationResult.RejectedByCurrent, e.Reason);
            }

            return new NavigationResultData(NavigationResult.Succeed);
        }

        private async Task HandleNavigatedTo(NavigationData navigationData)
        {
            //navigationData.RequestedModule.DataContext.Navigator.SetHostView(navigationData);

            if (navigationData.RequestedModule.DataContext is IOnNavigatedTo onNavigatedTo)
            {
                onNavigatedTo.OnNavigatedTo(navigationData);
            }

            if (navigationData.RequestedModule.DataContext is IOnNavigatedToAsync onNavigatedToAsync)
            {
                await onNavigatedToAsync.OnNavigatedToAsync(navigationData);
            }
        }

        private void InvokeNavigatedRequested(NavigationData navigationData)
        {
            navigationData.RequestedModule.SetDataContext(navigationData.RequestedModule.DataContext);
            NavigationRequested?.Invoke(this, navigationData);

            _currentView = navigationData.RequestedModule;
        }

        private void CleanUp(NavigationData navigationData)
        {
            if (navigationData.PreviousModule != _currentView)
            {
                navigationData.PreviousModule?.CleanUp();
            }
        }

        // We know that developers are lazy and so they won't implement properly Equals() on 
        // thier own implementation of INavigationModule, so we're providing a spereate one
        // just for NavigationService.
        private class NavigationModuleInternalComparer : IEqualityComparer<INavigationModule>
        {
            public bool Equals(INavigationModule x, INavigationModule y)
            {
                return x.DataContext.GetType() == y.DataContext.GetType();
            }

            public int GetHashCode(INavigationModule obj)
            {
                var hashCode = 536724180;
                hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(obj.View);
                hashCode = hashCode * -1521134295 + EqualityComparer<INavigatable>.Default.GetHashCode(obj.DataContext);
                return hashCode;
            }
        }
    }
}
