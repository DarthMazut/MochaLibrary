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

        public bool CanReturnModal
        {
            get
            {
                InitializationGuard();
                return _navigationService.NavigationHistory.Any(i => i.IsModalOrigin);
            }
        }

        public bool? SaveCurrent { get; set; }

        // Fluent API

        public Task<NavigationResultData> NavigateAsync(Func<INavigationDestinationBuilder, INavigationRequestDetailsBuilder> buildingDelegate)
        {
            NavigationRequestBuilder? builder 
                = buildingDelegate.Invoke(new NavigationRequestBuilder(_module)) as NavigationRequestBuilder ?? 
                throw new Exception();

            INavigationService service = builder.ResolveService() ?? _navigationService;
            return service.RequestNavigation(builder.Build());
        }

        // Default

        public Task<NavigationResultData> NavigateAsync(NavigationRequestData navigationRequestData)
            => _navigationService.RequestNavigation(navigationRequestData);

        // To

        public Task<NavigationResultData> NavigateAsync(string targetId)
             => NavigateAsync(targetId, null);

        public Task<NavigationResultData> NavigateAsync(string targetId, object? parameter)
            => _navigationService.RequestNavigation(NavigationRequestData.CreatePushRequest(targetId, _module, parameter, SaveCurrent, false, null));

        // Back

        public Task<NavigationResultData> NavigateBackAsync()
            => NavigateBackAsync(null);

        public Task<NavigationResultData> NavigateBackAsync(object? parameter)
            => NavigateBackAsync(parameter, 1);

        public Task<NavigationResultData> NavigateBackAsync(object? parameter, int step)
            => _navigationService.RequestNavigation(NavigationRequestData.CreateBackRequest(step, _module, parameter, SaveCurrent, false, null));

        // Forward

        public Task<NavigationResultData> NavigateForwardAsync()
            => NavigateForwardAsync(null);

        public Task<NavigationResultData> NavigateForwardAsync(object? parameter)
            => NavigateForwardAsync(parameter, 1);

        public Task<NavigationResultData> NavigateForwardAsync(object? parameter, int step)
            => _navigationService.RequestNavigation(NavigationRequestData.CreateForwardRequest(step, _module, parameter, SaveCurrent, false, null));

        // For Services

        public Task<NavigationResultData> NavigateAsyncForService(string targetServiceId, NavigationRequestData navigationRequestData)
        {
            INavigationService targetService = NavigationManager.FetchNavigationService(targetServiceId);
            return targetService.RequestNavigation(navigationRequestData);
        }

        public Task<NavigationResultData> NavigateAsyncForService(string targetServiceId, string targetId)
            => NavigateAsyncForService(targetServiceId, targetId, null);

        public Task<NavigationResultData> NavigateAsyncForService(string targetServiceId, string targetId, object? parameter)
        {
            INavigationService targetService = NavigationManager.FetchNavigationService(targetServiceId);
            return targetService.RequestNavigation(NavigationRequestData.CreatePushRequest(targetId, _module, parameter, SaveCurrent, false, null));
        }

        public Task<NavigationResultData> NavigateBackAsyncForService(string targetServiceId)
            => NavigateBackAsyncForService(targetServiceId, null);

        public Task<NavigationResultData> NavigateBackAsyncForService(string targetServiceId, object? parameter)
            => NavigateBackAsyncForService(targetServiceId, parameter, 1);

        public Task<NavigationResultData> NavigateBackAsyncForService(string targetServiceId, object? parameter, int step)
        {
            INavigationService targetService = NavigationManager.FetchNavigationService(targetServiceId);
            return targetService.RequestNavigation(NavigationRequestData.CreateBackRequest(step, _module, parameter, SaveCurrent, false, null));
        }

        public Task<NavigationResultData> NavigateForwardAsyncForService(string targetServiceId)
            => NavigateForwardAsyncForService(targetServiceId, null);

        public Task<NavigationResultData> NavigateForwardAsyncForService(string targetServiceId, object? parameter)
            => NavigateForwardAsyncForService(targetServiceId, parameter, 1);

        public Task<NavigationResultData> NavigateForwardAsyncForService(string targetServiceId, object? parameter, int step)
        {
            INavigationService targetService = NavigationManager.FetchNavigationService(targetServiceId);
            return targetService.RequestNavigation(NavigationRequestData.CreateForwardRequest(step, _module, parameter, SaveCurrent, false, null));
        }

        // Modal

        public Task<NavigationResultData> NavigateModalAsync(string targetId) => NavigateModalAsync(targetId, null);

        public Task<NavigationResultData> NavigateModalAsync(string targetId, object? parameter)
        {
            return _navigationService.RequestNavigation(
                NavigationRequestData.CreateModalRequest(
                    targetId,
                    _module,
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
            return _navigationService.RequestNavigation(NavigationRequestData.CreatePopRequest(
                _module,
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

        private void InitializationGuard()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException($"Cannot perform this action while {nameof(Navigator)} is not initialized.");
            }
        }
    }
}
