﻿using System;
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
        private NavigationFlowControl? _flowControl;
        
        protected NavigationStack<NavigationStackItem> _navigationStack = null!;

        /// <inheritdoc/>
        public event EventHandler<CurrentNavigationModuleChangedEventArgs>? CurrentModuleChanged;

        /// <inheritdoc/>
        public bool IsInitialized => _isInitialized;

        /// <inheritdoc/>
        public INavigationModule CurrentModule
        {
            get
            {
                InitializationGurad();
                return _navigationStack.CurrentItem.Module;
            }
        }

        /// <inheritdoc/>
        public INavigationStackItem CurrentItem
        {
            get
            {
                InitializationGurad();
                return _navigationStack.CurrentItem;
            }
        }

        /// <inheritdoc/>
        public IReadOnlyNavigationStack<INavigationStackItem> NavigationHistory
        {
            get
            {
                InitializationGurad();
                return _navigationStack.ToReadOnlyStack<INavigationStackItem>(m => m);
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
        public virtual Task Initialize(bool callSubscribersOnInit)
        {
            if (!_modules.Any())
            {
                throw new InvalidOperationException($"Cannot call {nameof(Initialize)} because no modules have been registered.");
            }

            if (_navigationStack is null)
            {
                _navigationStack = new NavigationStack<NavigationStackItem>(new NavigationStackItem(ResolveInitialModule()));
            }

            InitializeModule(_navigationStack.CurrentItem.Module);
            _isInitialized = true;
            
            if (callSubscribersOnInit)
            {
                OnNavigatedToEventArgs eventArgs = new(null, null, null);
                OnCurrentModuleChanged(new CurrentNavigationModuleChangedEventArgs(CurrentModule, eventArgs));
                return CallOnNavigatedTo(CurrentModule.DataContext, eventArgs);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public virtual void Uninitialize() => Uninitialize(false);

        /// <inheritdoc/>
        public virtual void Uninitialize(bool clearStack)
        {
            if (clearStack)
            {
                _navigationStack = null!;
                _modules.Select(kvp => kvp.Value).ToList().ForEach(module => module.Uninitialize());
                return;
            }

            bool preferCache = _navigationStack.CurrentItem.Module.LifecycleOptions.PreferCache;
            bool? saveCurrent = CurrentModule.DataContext?.Navigator.SaveCurrent;
            
            if (CurrentModule.IsInitialized)
            {
                if ((saveCurrent ?? preferCache) is false)
                {
                    _navigationStack.CurrentItem.Module.Uninitialize();
                }
            }

            _isInitialized = false;
        }

        /// <inheritdoc/>
        public virtual async Task<NavigationResultData> RequestNavigation(NavigationRequestData requestData)
        {
            InitializationGurad();
            ValidateRequestData(requestData);
            NavigationFlowControl flowControl = ResolveNavigationFlowControl();

            if (IsSameModuleRequested(requestData))
            {
                return new NavigationResultData(NavigationResult.SameModuleRequested);
            }

            bool canceled = await HandleOnNavigatingFrom(requestData);
            if (flowControl.ShouldAbort)
            {
                return new NavigationResultData(NavigationResult.RejectedByNewRequest);
            }

            if (canceled)
            {
                return new NavigationResultData(NavigationResult.RejectedByCurrent);
            }

            INavigationLifecycleModule previousModule = HandleNavigationRequest(requestData);
            await HandleOnNavigatedTo(previousModule, requestData);
            if (flowControl.ShouldAbort)
            {
                if ((requestData.SaveCurrent ?? previousModule.LifecycleOptions.PreferCache) is false)
                {
                    previousModule.Uninitialize();
                }
                return new NavigationResultData(NavigationResult.RejectedByNewRequest);
            }

            flowControl.CanAbort = false;
            await HandleOnNavigatedFrom(previousModule, requestData);
            flowControl.CanAbort = true;

            if (requestData.NavigationType == NavigationType.PushModal)
            {
                NavigationStackItem lastModalItem = _navigationStack.Last(i => i.IsModalOrigin);
                return new NavigationResultData(NavigationResult.Succeed, await lastModalItem.PopResultAsync());
            }

            return new NavigationResultData(NavigationResult.Succeed);
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

        protected virtual void ValidateRequestData(NavigationRequestData requestData)
        {
            if (requestData.NavigationType == NavigationType.Pop)
            {
                if (!_navigationStack.Any(i => i.IsModalOrigin))
                {
                    throw new ArgumentException("Cannot pop because modal stack is empty.", nameof(requestData));
                }
            }

            if (requestData.NavigationType == NavigationType.Push || requestData.NavigationType == NavigationType.PushModal)
            {
                if (_navigationStack.Where(i => i.IsModalOrigin).Any(i => i.Module.Id == requestData.TargetId))
                {
                    throw new ArgumentException("Cannot navigate to destination which originates modal request.", nameof(requestData));
                }
            }

            if (requestData.NavigationType == NavigationType.Push || requestData.NavigationType == NavigationType.PushModal)
            {
                if (!_modules.ContainsKey(requestData.TargetId!))
                {
                    throw new ArgumentException($"Destination with ID={requestData.TargetId} was never registered!", nameof(requestData));
                }
            }

            if (requestData.NavigationType == NavigationType.Back)
            {
                if (_navigationStack!.PeekBack(requestData.Step) is null)
                {
                    throw new ArgumentException("Cannot navigate before the first item.", nameof(requestData));
                }
            }

            if (requestData.NavigationType == NavigationType.Forward)
            {
                if (_navigationStack!.PeekForward(requestData.Step) is null)
                {
                    throw new ArgumentException("Cannot navigate forward beyond the last available item.", nameof(requestData));
                }
            }

            if (requestData.NavigationType == NavigationType.Back)
            {
                int lastModalItemIndex = GetLastModalItemIndex();
                if (lastModalItemIndex >= 0)
                {
                    int maxBackStep = _navigationStack.CurrentIndex - lastModalItemIndex - 1;
                    if (requestData.Step > maxBackStep)
                    {
                        throw new ArgumentException("Cannot navigate back to destination which originates modal request or before it.", nameof(requestData));
                    }
                }
            }
        }

        protected virtual INavigationLifecycleModule ResolveTargetModuleFromRequestData(NavigationRequestData requestData)
        {
            return requestData.NavigationType switch
            {
                NavigationType.Push => _modules[requestData.TargetId!],
                NavigationType.PushModal => _modules[requestData.TargetId!],
                NavigationType.Back => _navigationStack!.PeekBack(requestData.Step)!.Module,
                NavigationType.Forward => _navigationStack!.PeekForward(requestData.Step)!.Module,
                NavigationType.Pop => _navigationStack.Last(i => i.IsModalOrigin).Module,
                _ => throw new NotImplementedException()
            };
        }

        protected virtual bool IsSameModuleRequested(NavigationRequestData requestData)
            => CurrentModule == ResolveTargetModuleFromRequestData(requestData);

        protected virtual async Task<bool> HandleOnNavigatingFrom(NavigationRequestData requestData)
        {
            if (requestData.NavigationEventsOptions?.SupressNavigatingFromEvents == true)
            {
                return false;
            }

            OnNavigatingFromEventArgs eventArgs = new(
                    requestData.CallingModule,
                    ResolveTargetModuleFromRequestData(requestData)!,
                    requestData.Parameter,
                    requestData.NavigationType,
                    requestData.Step);

            await CallOnNavigatingFrom(CurrentModule.DataContext, eventArgs);
            return eventArgs.Cancel;
        }

        protected virtual INavigationLifecycleModule HandleNavigationRequest(NavigationRequestData requestData)
        {
            INavigationLifecycleModule previousModule = _navigationStack.CurrentItem.Module;

            if (requestData.NavigationType == NavigationType.Push)
            {
                _navigationStack.Push(new NavigationStackItem(ResolveTargetModuleFromRequestData(requestData)));
            }

            if (requestData.NavigationType == NavigationType.PushModal)
            {
                _navigationStack.CurrentItem.SetModal();
                _navigationStack.Push(new NavigationStackItem(ResolveTargetModuleFromRequestData(requestData)));
            }

            if (requestData.NavigationType == NavigationType.Pop)
            {
                _ = _navigationStack.Pop(_navigationStack.CurrentIndex - GetLastModalItemIndex());
            }

            if (requestData.NavigationType == NavigationType.Back)
            {
                _navigationStack.MoveBack(requestData.Step);
            }
            
            if (requestData.NavigationType == NavigationType.Forward)
            {
                _navigationStack.MoveForward(requestData.Step);
            }

            if (requestData.IgnoreCached)
            {
                _navigationStack.CurrentItem.Module.Uninitialize();
            }

            if (!CurrentModule.IsInitialized)
            {
                InitializeModule(_navigationStack.CurrentItem.Module);
            }

            return previousModule;
        }

        protected virtual async Task HandleOnNavigatedTo(INavigationLifecycleModule previousModule, NavigationRequestData requestData)
        {
            OnNavigatedToEventArgs eventArgs = new OnNavigatedToEventArgs(
                    requestData.CallingModule,
                    previousModule,
                    requestData.Parameter,
                    requestData.NavigationType,
                    requestData.Step);

            OnCurrentModuleChanged(new CurrentNavigationModuleChangedEventArgs(CurrentModule, eventArgs));
            
            if (requestData.NavigationType == NavigationType.Pop)
            {
                _navigationStack.CurrentItem.SetResult(requestData.Parameter);
            }
            else
            {
                ISetNavigationContext? contextSetter = CurrentModule.DataContext?.Navigator;
                contextSetter?.SetNavigationContext(eventArgs);
            }

            if (requestData.NavigationEventsOptions?.SupressNavigatedToEvents != true)
            {
                await CallOnNavigatedTo(CurrentModule.DataContext, eventArgs);
            }
        }

        protected virtual async Task HandleOnNavigatedFrom(INavigationLifecycleModule previousModule, NavigationRequestData requestData)
        {
            if (requestData.NavigationEventsOptions?.SupressNavigatedFromEvents != true)
            {
                await CallOnNavigatedFrom(previousModule.DataContext, new OnNavigatedFromEventArgs(
                    requestData.CallingModule,
                    CurrentModule,
                    requestData.Parameter,
                    requestData.NavigationType,
                    requestData.Step));
            }

            if ((requestData.SaveCurrent ?? previousModule.LifecycleOptions.PreferCache) is false)
            {
                previousModule.Uninitialize();
            }
        }

        protected virtual void InitializeModule(INavigationLifecycleModule module)
        {
            if (!module.IsInitialized)
            {
                module.Initialize();
                (module.DataContext?.Navigator as INavigatorInitialize)?.Initialize(module, this);
            }
        }

        protected virtual INavigationLifecycleModule ResolveInitialModule()
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

        protected int GetLastModalItemIndex()
        {
            int lastModalIndex = -1;
            for (int i = _navigationStack.CurrentIndex; i >= 0; i--)
            {
                if (_navigationStack.InternalCollection[i].IsModalOrigin)
                {
                    lastModalIndex = i;
                    break;
                }
            }

            return lastModalIndex;
        }

        protected virtual Task CallOnNavigatingFrom(INavigatable? dataContext, OnNavigatingFromEventArgs e)
        {
            (dataContext as IOnNavigatingFrom)?.OnNavigatingFrom(e);
            return (dataContext as IOnNavigatingFromAsync)?.OnNavigatingFromAsync(e) ?? Task.CompletedTask;
        }

        protected virtual Task CallOnNavigatedTo(INavigatable? dataContext, OnNavigatedToEventArgs e)
        {
            (dataContext as IOnNavigatedTo)?.OnNavigatedTo(e);
            return (dataContext as IOnNavigatedToAsync)?.OnNavigatedToAsync(e) ?? Task.CompletedTask;
        }

        protected virtual Task CallOnNavigatedFrom(INavigatable? dataContext, OnNavigatedFromEventArgs e)
        {
            (dataContext as IOnNavigatedFrom)?.OnNavigatedFrom(e);
            return (dataContext as IOnNavigatedFromAsync)?.OnNavigatedFromAsync(e) ?? Task.CompletedTask;
        }

        protected virtual void OnCurrentModuleChanged(CurrentNavigationModuleChangedEventArgs args)
        {
            CurrentModuleChanged?.Invoke(this, args);
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
