using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Exposes API for navigation.
    /// </summary>
    public class Navigator : IDisposable, INavigatorInitialize, ISetNavigationContext
    {
        private bool _isInitialized;
        protected INavigationModule _module = null!;
        protected INavigationService _navigationService = null!;
        private OnNavigatedToEventArgs? _context;

        public event EventHandler<NavigatorInitializedEventArgs>? Initialized;

        public bool IsInitialized => _isInitialized;

        public OnNavigatedToEventArgs? Context => _context;

        public INavigationModule Module => _module;

        public INavigationService Service => _navigationService;

        public bool CanNavigateBack 
        { 
            get
            {
                InitializationGuard();
                return !_navigationService.NavigationHistory.IsBottomIndex;
            }
        }

        public bool CanNavigateForward
        { 
            get
            {
                InitializationGuard();
                return !_navigationService.NavigationHistory.IsTopIndex;
            }
        }

        public bool? SaveCurrent { get; set; }

        public Task NavigateAsync(Func<INavigationDestinationBuilder, INavigationRequestDetailsBuilder> buildingDelegate)
        {
            NavigationRequestBuilder? builder 
                = buildingDelegate.Invoke(new NavigationRequestBuilder(_module)) as NavigationRequestBuilder ?? 
                throw new Exception();

            INavigationService service = builder.ResolveService() ?? _navigationService;
            return service.RequestNavigation(builder.Build());
        }

        public Task NavigateAsync(string targetId)
             => NavigateAsync(targetId, null);

        public Task NavigateAsync(string targetId, object? parameter)
            => _navigationService.RequestNavigation(new NavigationRequestData(targetId, _module, parameter, SaveCurrent, false));

        public Task NavigateAsync(NavigationRequestData navigationRequestData)
            => _navigationService.RequestNavigation(navigationRequestData);

        public Task NavigateBackAsync()
            => NavigateBackAsync(null);

        public Task NavigateBackAsync(object? parameter)
            => NavigateBackAsync(parameter, 1);

        public Task NavigateBackAsync(object? parameter, int step)
            => _navigationService.RequestNavigation(new NavigationRequestData(NavigationType.Back, step, _module, parameter, SaveCurrent, false));

        public Task NavigateForwardAsync()
            => NavigateForwardAsync(null);

        public Task NavigateForwardAsync(object? parameter)
            => NavigateForwardAsync(parameter, 1);

        public Task NavigateForwardAsync(object? parameter, int step)
            => _navigationService.RequestNavigation(new NavigationRequestData(NavigationType.Forward, step, _module, parameter, SaveCurrent, false));

        void INavigatorInitialize.Initialize(INavigationModule module, INavigationService navigationService)
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException($"Cannot initialize {nameof(Navigator)} instance because it was already initalized.");
            }

            _module = module;
            _navigationService = navigationService;
            _isInitialized = true;
            Initialized?.Invoke(this, new NavigatorInitializedEventArgs());
        }

        /// <inheritdoc/>
        void ISetNavigationContext.SetNavigationContext(OnNavigatedToEventArgs context) => _context = context;

        public void Dispose()
        {
            _module = null!;
            _navigationService = null!;
            _context = null!;
            GC.SuppressFinalize(this);
        }

        private void InitializationGuard()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException($"Cannot perform this action while {nameof(Navigator)} is not initialized.");
            }
        }
    }
}
