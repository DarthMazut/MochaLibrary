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
        private readonly object? _owner;

        public Navigator() : this(null) { }

        public Navigator(object? owner)
        {
            _owner = owner;
        }

        public event EventHandler<NavigatorInitializedEventArgs>? Initialized;

        public bool IsInitialized => _isInitialized;

        public OnNavigatedToEventArgs? Context => _context;

        public object? Owner => _owner;

        public INavigationModule Module => _module;

        public INavigationService Service => _navigationService;

        public bool CanNavigateBack 
        { 
            get
            {
                ThrowIfNavigationServiceIsNull();
                return !_navigationService.NavigationHistory.IsBottomIndex;
            }
        }

        public bool CanNavigateForward
        { 
            get
            {
                ThrowIfNavigationServiceIsNull();
                return !_navigationService.NavigationHistory.IsTopIndex;
            }
        }

        public bool CanReturnModal
        {
            get
            {
                ThrowIfNavigationServiceIsNull();
                return _navigationService.NavigationHistory.Any(i => i.IsModalOrigin);
            }
        }

        public bool? SaveCurrent { get; set; }

        // Fluent API

        public Task<NavigationResultData> NavigateAsync(Func<INavigationDestinationBuilder, INavigationRequestDetailsBuilder> buildingDelegate)
        {
            ThrowIfNavigationServiceIsNull();
            NavigationRequestBuilder builder = (buildingDelegate.Invoke(new NavigationRequestBuilder(GetModuleOrOwner())) as NavigationRequestBuilder)!;
            return _navigationService.RequestNavigation(builder.Build());
        }

        // Default

        public Task<NavigationResultData> NavigateAsync(NavigationRequestData navigationRequestData)
        {
            ThrowIfNavigationServiceIsNull();
            return _navigationService.RequestNavigation(navigationRequestData);
        }

        // To

        public Task<NavigationResultData> NavigateAsync(string targetId)
             => NavigateAsync(targetId, null);

        public Task<NavigationResultData> NavigateAsync(string targetId, object? parameter)
        {
            ThrowIfNavigationServiceIsNull();
            return _navigationService.RequestNavigation(NavigationRequestData.CreatePushRequest(targetId, GetModuleOrOwner(), parameter, SaveCurrent, false, null));
        }

        // Back

        public Task<NavigationResultData> NavigateBackAsync()
            => NavigateBackAsync(null);

        public Task<NavigationResultData> NavigateBackAsync(object? parameter)
            => NavigateBackAsync(parameter, 1);

        public Task<NavigationResultData> NavigateBackAsync(object? parameter, int step)
        {
            ThrowIfNavigationServiceIsNull();
            return _navigationService.RequestNavigation(NavigationRequestData.CreateBackRequest(step, GetModuleOrOwner(), parameter, SaveCurrent, false, null));
        }

        // Forward

        public Task<NavigationResultData> NavigateForwardAsync()
            => NavigateForwardAsync(null);

        public Task<NavigationResultData> NavigateForwardAsync(object? parameter)
            => NavigateForwardAsync(parameter, 1);

        public Task<NavigationResultData> NavigateForwardAsync(object? parameter, int step)
        {
            ThrowIfNavigationServiceIsNull();
            return _navigationService.RequestNavigation(NavigationRequestData.CreateForwardRequest(step, GetModuleOrOwner(), parameter, SaveCurrent, false, null));
        }

        // Modal

        public Task<NavigationResultData> NavigateModalAsync(string targetId) => NavigateModalAsync(targetId, null);

        public Task<NavigationResultData> NavigateModalAsync(string targetId, object? parameter)
        {
            ThrowIfNavigationServiceIsNull();
            return _navigationService.RequestNavigation(
                NavigationRequestData.CreateModalRequest(
                    targetId,
                    GetModuleOrOwner(),
                    parameter,
                    false,
                    new NavigationEventsOptions()
                    { 
                        SupressNavigatedFromEvents = true
                    }));
        }

        // Return Modal

        public Task ReturnModal() => ReturnModal(null, null);

        public Task ReturnModal(object returnData) => ReturnModal(returnData, null);

        public Task ReturnModal(object? returnData, NavigationEventsOptions? eventsOptions)
        {
            ThrowIfNavigationServiceIsNull();
            return _navigationService.RequestNavigation(NavigationRequestData.CreatePopRequest(
                GetModuleOrOwner(),
                returnData,
                eventsOptions ?? new NavigationEventsOptions()
                {
                    SupressNavigatedToEvents = true
                }));
        }

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

        private object? GetModuleOrOwner() => _module ?? _owner;

        private void ThrowIfNavigationServiceIsNull()
        {
            if (_navigationService is null)
            {
                throw new InvalidOperationException($"Cannot perform this action while {nameof(Navigator)} is not initialized.");
            }
        }
    }
}
