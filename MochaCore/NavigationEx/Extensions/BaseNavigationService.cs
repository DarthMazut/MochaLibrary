using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx.Extensions
{
    public abstract class BaseNavigationService : INavigationService
    {
        private bool _isInitialized;
        private string? _initialId;
        private INavigationLifecycleModule? _rootModule;
        private readonly Dictionary<string, INavigationLifecycleModule> _modules = new();
        private NavigationStack<INavigationLifecycleModule> _navigationStack = null!;
        private NavigationFlowControl? _flowControl;

        /// <inheritdoc/>
        public bool IsInitialized => _isInitialized;

        /// <inheritdoc/>
        public INavigationModule CurrentModule
        {
            get
            {
                InitializationGurad();
                return _navigationStack.CurrentItem;
            }
        }

        /// <inheritdoc/>
        public IReadOnlyNavigationStack<INavigationModule> NavigationHistory
        {
            get
            {
                InitializationGurad();
                return _navigationStack.ToReadOnlyStack<INavigationModule>(m => m);
            }
        }

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, INavigationModule> AvailableModules
        {
            get 
            {
                Dictionary<string, INavigationModule> modules = _modules.ToDictionary(kvp => kvp.Key, kvp => kvp.Value as INavigationModule);
                if (_rootModule is not null)
                {
                    modules.Add(_rootModule.Id, _rootModule);
                }
                return new ReadOnlyDictionary<string, INavigationModule>(modules);
            }
        }

        /// <inheritdoc/>
        public event EventHandler<CurrentNavigationModuleChangedEventArgs>? CurrentModuleChanged;

        public void AddModule(INavigationLifecycleModule module)
        {
            if (_modules.ContainsKey(module.Id))
            {
                throw new ArgumentException($"Provided ID was already registered.");
            }

            if (module.Id == INavigationModule.RootId)
            {
                _rootModule = module;
                (_rootModule.DataContext?.Navigator as INavigatorInitialize)?.Initialize(module, this);
            }
            else
            {
                _modules.Add(module.Id, module);
            }
        }

        public void SelectInitialtId(string id)
        {
            _initialId = id;
        }

        /// <inheritdoc/>
        public Task Initialize() => Initialize(true);

        /// <inheritdoc/>
        public Task Initialize(bool callSubscribersOnInit)
        {
            if (!_modules.Any())
            {
                throw new InvalidOperationException($"Cannot call {nameof(Initialize)} because no modules have been registered.");
            }

            if (_navigationStack is null)
            {
                _navigationStack = new NavigationStack<INavigationLifecycleModule>(ResolveInitialModule());
            }

            InitializeModule(_navigationStack.CurrentItem);
            _isInitialized = true;
            
            if (callSubscribersOnInit)
            {
                OnNavigatedToEventArgs eventArgs = new(null, null, null);
                CurrentModuleChanged?.Invoke(this, new CurrentNavigationModuleChangedEventArgs(CurrentModule, eventArgs));
                return CallOnNavigatedTo(_navigationStack.CurrentItem.DataContext, eventArgs);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void Uninitialize() => Uninitialize(false);

        /// <inheritdoc/>
        public void Uninitialize(bool clearStack)
        {
            if (clearStack)
            {
                _navigationStack = null!;
                _modules.Select(kvp => kvp.Value).ToList().ForEach(module => module.Uninitialize());
                return;
            }

            bool preferCache = _navigationStack.CurrentItem.LifecycleOptions.PreferCache;
            bool? saveCurrent = _navigationStack.CurrentItem.DataContext?.Navigator.SaveCurrent;
            
            if (_navigationStack.CurrentItem.IsInitialized)
            {
                if (saveCurrent is null)
                {
                    if (preferCache == false)
                    {
                        _navigationStack.CurrentItem.Uninitialize();
                    }
                }
                else
                {
                    if (saveCurrent == false)
                    {
                        _navigationStack.CurrentItem.Uninitialize();
                    }
                }
            }

            _isInitialized = false;
        }

        /// <inheritdoc/>
        public async Task<NavigationResult> RequestNavigation(NavigationRequestData requestData)
        {
            InitializationGurad();
            ValidateRequestData(requestData);
            NavigationFlowControl flowControl = ResolveNavigationFlowControl();

            if (IsSameModuleRequested(requestData))
            {
                return NavigationResult.SameModuleRequested;
            }

            bool canceled = await HandleOnNavigatingFrom(requestData);
            if (flowControl.ShouldAbort)
            {
                return NavigationResult.RejectedByNewRequest;
            }

            if (canceled)
            {
                return NavigationResult.RejectedByCurrent;
            }

            INavigationLifecycleModule previousModule = HandleNavigationRequest(requestData);
            await HandleOnNavigatedTo(previousModule, requestData);
            if (flowControl.ShouldAbort)
            {
                if ((requestData.SaveCurrent ?? previousModule.LifecycleOptions.PreferCache) is false)
                {
                    previousModule.Uninitialize();
                }
                return NavigationResult.RejectedByNewRequest;
            }

            flowControl.CanAbort = false;
            await HandleOnNavigatedFrom(previousModule, requestData);
            flowControl.CanAbort = true;

            return NavigationResult.Succeed;
        }

        private NavigationFlowControl ResolveNavigationFlowControl()
        {
            if (_flowControl?.CanAbort == false)
            {
                throw new InvalidOperationException("Cannot redirect navigation at this stage.");
            }

            _flowControl?.Abort();
            _flowControl = new NavigationFlowControl();
            return _flowControl;
        }

        private void ValidateRequestData(NavigationRequestData requestData)
        {
            if (requestData.NavigationType == NavigationType.Push && !_modules.ContainsKey(requestData.TargetId!))
            {
                throw new ArgumentException($"Destination with ID={requestData.TargetId} was never registered!", nameof(requestData));
            }

            if (requestData.NavigationType == NavigationType.Back)
            {
                if (_navigationStack!.PeekBack(requestData.Step) is null)
                {
                    throw new ArgumentException("Navigation request data was invalid.");
                }
            }

            if (requestData.NavigationType == NavigationType.Forward)
            {
                if (_navigationStack!.PeekForward(requestData.Step) is null)
                {
                    throw new ArgumentException("Navigation request data was invalid.");
                }
            }
        }

        private INavigationLifecycleModule ResolveTargetModuleFromRequestData(NavigationRequestData requestData)
        {
            return requestData.NavigationType switch
            {
                NavigationType.Push => _modules[requestData.TargetId!],
                NavigationType.Back => _navigationStack!.PeekBack(requestData.Step)!,
                NavigationType.Forward => _navigationStack!.PeekForward(requestData.Step)!,
                _ => throw new NotImplementedException()
            };
        }

        private bool IsSameModuleRequested(NavigationRequestData requestData)
            => _navigationStack.CurrentItem == ResolveTargetModuleFromRequestData(requestData);

        private async Task<bool> HandleOnNavigatingFrom(NavigationRequestData requestData)
        {
            OnNavigatingFromEventArgs eventArgs = new(
                    requestData.CallingModule,
                    ResolveTargetModuleFromRequestData(requestData)!,
                    requestData.Parameter,
                    requestData.NavigationType,
                    requestData.Step);

            await CallOnNavigatingFrom(_navigationStack.CurrentItem.DataContext, eventArgs);
            return eventArgs.Cancel;
        }

        private INavigationLifecycleModule HandleNavigationRequest(NavigationRequestData requestData)
        {
            INavigationLifecycleModule previousModule = _navigationStack.CurrentItem;

            if (requestData.NavigationType == NavigationType.Push)
            {
                _navigationStack.Push(ResolveTargetModuleFromRequestData(requestData));
            }
            else
            {
                if (requestData.NavigationType == NavigationType.Back)
                {
                    _navigationStack.MoveBack(requestData.Step);
                }

                if (requestData.NavigationType == NavigationType.Forward)
                {
                    _navigationStack.MoveForward(requestData.Step);
                }
            }

            if (requestData.IgnoreCached)
            {
                _navigationStack.CurrentItem.Uninitialize();
            }

            if (!_navigationStack.CurrentItem.IsInitialized)
            {
                InitializeModule(_navigationStack.CurrentItem);
            }

            return previousModule;
        }

        private Task HandleOnNavigatedTo(INavigationLifecycleModule previousModule, NavigationRequestData requestData)
        {
            OnNavigatedToEventArgs eventArgs = new OnNavigatedToEventArgs(
                    requestData.CallingModule,
                    previousModule,
                    requestData.Parameter,
                    requestData.NavigationType,
                    requestData.Step);

            CurrentModuleChanged?.Invoke(this, new CurrentNavigationModuleChangedEventArgs(CurrentModule, eventArgs));
            ISetNavigationContext? contextSetter = _navigationStack.CurrentItem.DataContext?.Navigator;
            contextSetter?.SetNavigationContext(eventArgs);
            return CallOnNavigatedTo(_navigationStack.CurrentItem.DataContext, eventArgs);
        }

        private async Task HandleOnNavigatedFrom(INavigationLifecycleModule previousModule, NavigationRequestData requestData)
        {
            await CallOnNavigatedFrom(previousModule.DataContext, new OnNavigatedFromEventArgs(
                    requestData.CallingModule,
                    _navigationStack.CurrentItem,
                    requestData.Parameter,
                    requestData.NavigationType,
                    requestData.Step));

            if ((requestData.SaveCurrent ?? previousModule.LifecycleOptions.PreferCache) is false)
            {
                previousModule.Uninitialize();
            }
        }

        private void InitializeModule(INavigationLifecycleModule? module)
        {
            if (module?.IsInitialized == false)
            {
                module.Initialize();
                (module.DataContext?.Navigator as INavigatorInitialize)?.Initialize(module, this);
            }
        }

        private INavigationLifecycleModule ResolveInitialModule()
        {
            if (_initialId is null)
            {
                return _modules.First().Value;
            }
            else
            {
                if (!_modules.ContainsKey(_initialId))
                {
                    throw new ArgumentException($"Initial module with id={_initialId} cannot be found. Make sure it's registered!");
                }

                return _modules[_initialId];
            }
        }

        private Task CallOnNavigatingFrom(INavigatable? dataContext, OnNavigatingFromEventArgs e)
        {
            (dataContext as IOnNavigatingFrom)?.OnNavigatingFrom(e);
            return (dataContext as IOnNavigatingFromAsync)?.OnNavigatingFromAsync(e) ?? Task.CompletedTask;
        }

        private Task CallOnNavigatedTo(INavigatable? dataContext, OnNavigatedToEventArgs e)
        {
            (dataContext as IOnNavigatedTo)?.OnNavigatedTo(e);
            return (dataContext as IOnNavigatedToAsync)?.OnNavigatedToAsync(e) ?? Task.CompletedTask;
        }

        private Task CallOnNavigatedFrom(INavigatable? dataContext, OnNavigatedFromEventArgs e)
        {
            (dataContext as IOnNavigatedFrom)?.OnNavigatedFrom(e);
            return (dataContext as IOnNavigatedFromAsync)?.OnNavigatedFromAsync(e) ?? Task.CompletedTask;
        }

        private void InitializationGurad()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException($"Navigation service hasn't been initialized");
            }
        }

        private class NavigationFlowControl
        {
            private bool _shouldAbort;

            public bool CanAbort { get; set; } = true;
            public bool ShouldAbort => _shouldAbort;
            internal void Abort()
            {
                _shouldAbort = true;
            }
        }
    }
}
