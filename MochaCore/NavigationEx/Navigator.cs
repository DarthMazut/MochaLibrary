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
                return _navigationService.ModalNavigationStack.Any();
            }
        }

        public bool? SaveCurrent { get; set; }

        public Task<NavigationResult> NavigateAsync(Func<INavigationDestinationBuilder, INavigationRequestDetailsBuilder> buildingDelegate)
        {
            NavigationRequestBuilder? builder 
                = buildingDelegate.Invoke(new NavigationRequestBuilder(_module)) as NavigationRequestBuilder ?? 
                throw new Exception();

            INavigationService service = builder.ResolveService() ?? _navigationService;
            return service.RequestNavigation(builder.Build());
        }

        public Task<NavigationResult> NavigateAsync(string targetId)
             => NavigateAsync(targetId, null);

        public Task<NavigationResult> NavigateAsync(string targetId, object? parameter)
            => _navigationService.RequestNavigation(new NavigationRequestData(targetId, _module, parameter, SaveCurrent, false));

        public Task<NavigationResult> NavigateAsync(NavigationRequestData navigationRequestData)
            => _navigationService.RequestNavigation(navigationRequestData);

        public Task<NavigationResult> NavigateBackAsync()
            => NavigateBackAsync(null);

        public Task<NavigationResult> NavigateBackAsync(object? parameter)
            => NavigateBackAsync(parameter, 1);

        public Task<NavigationResult> NavigateBackAsync(object? parameter, int step)
            => _navigationService.RequestNavigation(new NavigationRequestData(NavigationType.Back, step, _module, parameter, SaveCurrent, false));

        public Task<NavigationResult> NavigateForwardAsync()
            => NavigateForwardAsync(null);

        public Task<NavigationResult> NavigateForwardAsync(object? parameter)
            => NavigateForwardAsync(parameter, 1);

        public Task<NavigationResult> NavigateForwardAsync(object? parameter, int step)
            => _navigationService.RequestNavigation(new NavigationRequestData(NavigationType.Forward, step, _module, parameter, SaveCurrent, false));

        public Task<NavigationResult> NavigateAsyncForService(string targetServiceId, string targetId)
            => NavigateAsyncForService(targetServiceId, targetId, null);

        public Task<NavigationResult> NavigateAsyncForService(string targetServiceId, string targetId, object? parameter)
        {
            INavigationService targetService = NavigationManager.FetchNavigationService(targetServiceId);
            return targetService.RequestNavigation(new NavigationRequestData(targetId, _module, parameter, SaveCurrent, false));
        }

        public Task<NavigationResult> NavigateAsyncForService(string targetServiceId, NavigationRequestData navigationRequestData)
        {
            INavigationService targetService = NavigationManager.FetchNavigationService(targetServiceId);
            return targetService.RequestNavigation(navigationRequestData);
        }

        public Task<NavigationResult> NavigateBackAsyncForService(string targetServiceId)
            => NavigateBackAsyncForService(targetServiceId, null);

        public Task<NavigationResult> NavigateBackAsyncForService(string targetServiceId, object? parameter)
            => NavigateBackAsyncForService(targetServiceId, parameter, 1);

        public Task<NavigationResult> NavigateBackAsyncForService(string targetServiceId, object? parameter, int step)
        {
            INavigationService targetService = NavigationManager.FetchNavigationService(targetServiceId);
            return targetService.RequestNavigation(new NavigationRequestData(NavigationType.Back, step, _module, parameter, SaveCurrent, false));
        }

        public Task<NavigationResult> NavigateForwardAsyncForService(string targetServiceId)
            => NavigateForwardAsyncForService(targetServiceId, null);

        public Task<NavigationResult> NavigateForwardAsyncForService(string targetServiceId, object? parameter)
            => NavigateForwardAsyncForService(targetServiceId, parameter, 1);

        public Task<NavigationResult> NavigateForwardAsyncForService(string targetServiceId, object? parameter, int step)
        {
            INavigationService targetService = NavigationManager.FetchNavigationService(targetServiceId);
            return targetService.RequestNavigation(new NavigationRequestData(NavigationType.Forward, step, _module, parameter, SaveCurrent, false));
        }

        public Task NavigateModalAsync(string targetId) => NavigateModalAsync(targetId, null);

        public async Task<object> NavigateModalAsync(string targetId, object? parameter)
        {
            TaskCompletionSource<object> tsc = new();
            await _navigationService.RequestNavigation(NavigationRequestData.CreateModalRequest(targetId, _module, parameter, tsc));
            return tsc.Task;
        }

        public Task ReturnModal() => ReturnModal(null, null);

        public Task ReturnModal(object returnData) => ReturnModal(returnData, null);

        public Task ReturnModal(object returnData, bool supressOnNavigatedToEvent)
            => ReturnModal(returnData, new NavigationEventsOptions()
            {
                SupressNavigatedToEvents = supressOnNavigatedToEvent
            });

        public async Task ReturnModal(object? returnData, NavigationEventsOptions? navigationEventsOptions)
        {
            if (!_navigationService.ModalNavigationStack.Any())
            {
                throw new InvalidOperationException("No modal navigation can be popped from modal navigation stack at this time.");
            }

            ModalNavigationData navigationData = _navigationService.ModalNavigationStack.Pop();
            await _navigationService.RequestNavigation(NavigationRequestData.CreatePopRequest(
                navigationData,
                _module,
                navigationEventsOptions ?? new NavigationEventsOptions()
                {
                    SupressNavigatedToEvents = true
                }));
            navigationData.ModalNavigationCompletionSource.SetResult(returnData ?? new object());
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
