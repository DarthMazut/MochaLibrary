using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Navigation
{
    /// <summary>
    /// Exposes API for navigation.
    /// </summary>
    public class Navigator : INavigator, INavigatorInitialize, ISetNavigationContext, IDisposable
    {
        private bool _isInitialized;
        protected INavigationModule _module = null!;
        protected INavigationService _navigationService = null!;
        private OnNavigatedToEventArgs? _context;
        private readonly object? _owner;

        protected Navigator(object? owner, INavigationService? service)
        {
            _owner = owner;
            _navigationService = service!;
        }

        /// <summary>
        /// Returns implementation of <see cref="INavigator"/> dedicated for <see cref="INavigationParticipant"/> implementations.
        /// </summary>
        public static INavigator Create() => new Navigator(null, null);

        /// <summary>
        /// Returns implementation of <see cref="IRemoteNavigator"/> dedicated for proxy navigation clients.
        /// </summary>
        /// <param name="serviceId">Identifier of <see cref="INavigationService"/> which handles proxy navigation requests.</param>
        /// <param name="owner">An object that initiates navigation using the created instance.</param>
        public static IRemoteNavigator CreateProxy(string serviceId, object? owner)
            => new Navigator(owner, NavigationManager.FetchNavigationService(serviceId));

        /// <inheritdoc/>
        public event EventHandler<NavigatorInitializedEventArgs>? Initialized;

        /// <inheritdoc/>
        public bool IsInitialized => _isInitialized;

        /// <inheritdoc/>
        public OnNavigatedToEventArgs? Context => _context;

        /// <inheritdoc/>
        public object? Owner => _owner;

        /// <inheritdoc/>
        public INavigationModule Module => _module;

        /// <inheritdoc/>
        public INavigationService Service => _navigationService;

        /// <inheritdoc/>
        public bool CanNavigateBack
        {
            get
            {
                ThrowIfNavigationServiceIsNull();
                return !_navigationService.NavigationHistory.IsBottomIndex;
            }
        }

        /// <inheritdoc/>
        public bool CanNavigateForward
        {
            get
            {
                ThrowIfNavigationServiceIsNull();
                return !_navigationService.NavigationHistory.IsTopIndex;
            }
        }

        /// <inheritdoc/>
        public bool CanReturnModal
        {
            get
            {
                ThrowIfNavigationServiceIsNull();
                return _navigationService.NavigationHistory.Any(i => i.IsModalOrigin);
            }
        }

        /// <inheritdoc/>
        public bool? SaveCurrent { get; set; }

        // Fluent API

        /// <inheritdoc/>
        public Task<NavigationResultData> NavigateAsync(Func<INavigationDestinationBuilder, INavigationRequestDetailsBuilder> buildingDelegate)
        {
            ThrowIfNavigationServiceIsNull();
            NavigationRequestBuilder builder = (buildingDelegate.Invoke(new NavigationRequestBuilder(GetModuleOrOwner())) as NavigationRequestBuilder)!;
            return _navigationService.RequestNavigation(builder.Build());
        }

        // Default

        /// <inheritdoc/>
        public Task<NavigationResultData> NavigateAsync(NavigationRequestData navigationRequestData)
        {
            ThrowIfNavigationServiceIsNull();
            return _navigationService.RequestNavigation(navigationRequestData);
        }

        // To

        /// <inheritdoc/>
        public Task<NavigationResultData> NavigateAsync(string targetId)
             => NavigateAsync(targetId, null);

        /// <inheritdoc/>
        public Task<NavigationResultData> NavigateAsync(string targetId, object? parameter)
        {
            ThrowIfNavigationServiceIsNull();
            return _navigationService.RequestNavigation(NavigationRequestData.CreatePushRequest(targetId, GetModuleOrOwner(), parameter, SaveCurrent, false, null));
        }

        // Back

        /// <inheritdoc/>
        public Task<NavigationResultData> NavigateBackAsync()
            => NavigateBackAsync(null);

        /// <inheritdoc/>
        public Task<NavigationResultData> NavigateBackAsync(object? parameter)
            => NavigateBackAsync(parameter, 1);

        /// <inheritdoc/>
        public Task<NavigationResultData> NavigateBackAsync(object? parameter, int step)
        {
            ThrowIfNavigationServiceIsNull();
            return _navigationService.RequestNavigation(NavigationRequestData.CreateBackRequest(step, GetModuleOrOwner(), parameter, SaveCurrent, false, null));
        }

        // Forward

        /// <inheritdoc/>
        public Task<NavigationResultData> NavigateForwardAsync()
            => NavigateForwardAsync(null);

        /// <inheritdoc/>
        public Task<NavigationResultData> NavigateForwardAsync(object? parameter)
            => NavigateForwardAsync(parameter, 1);

        /// <inheritdoc/>
        public Task<NavigationResultData> NavigateForwardAsync(object? parameter, int step)
        {
            ThrowIfNavigationServiceIsNull();
            return _navigationService.RequestNavigation(NavigationRequestData.CreateForwardRequest(step, GetModuleOrOwner(), parameter, SaveCurrent, false, null));
        }

        // Modal

        /// <inheritdoc/>
        public Task<object?> NavigateModalAsync(string targetId) => NavigateModalAsync(targetId, null);

        /// <inheritdoc/>
        public Task<object?> NavigateModalAsync(string targetId, object? parameter)
        {
            ThrowIfNavigationServiceIsNull();
            return _navigationService.RequestModalNavigation(
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

        /// <inheritdoc/>
        public Task ReturnModal() => ReturnModal(null, null);

        /// <inheritdoc/>
        public Task ReturnModal(object returnData) => ReturnModal(returnData, null);

        /// <inheritdoc/>
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
