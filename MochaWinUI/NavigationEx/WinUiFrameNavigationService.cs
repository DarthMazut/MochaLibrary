using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using MochaCore.NavigationEx;
using MochaCore.NavigationEx.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.NavigationEx
{
    /// <summary>
    /// Provides implementation of <see cref="INavigationService"/> base on
    /// <see cref="Frame"/> object.
    /// </summary>
    public class WinUiFrameNavigationService : BaseNavigationService
    {
        private Func<Frame>? _frameDelegate;

        public WinUiFrameNavigationService WithModule<TView, TDataContext>(string id)
           where TView : FrameworkElement
           where TDataContext : class, INavigatable
               => WithModule<TView, TDataContext>(id, new NavigationModuleLifecycleOptions());

        public WinUiFrameNavigationService WithModule<TView, TDataContext>(string id, NavigationModuleLifecycleOptions lifecycleOptions)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
        {
            if (typeof(TView).GetConstructor(Type.EmptyTypes) is null)
            {
                throw new ArgumentException("Provided view type does not have parameterless constructor and cannot be registered " +
                    $"by this overload of {nameof(WithModule)} method.");
            }

            if (typeof(TDataContext).GetConstructor(Type.EmptyTypes) is null)
            {
                throw new ArgumentException("Provided dataContext type does not have parameterless constructor and cannot be registered " +
                    $"by this overload of {nameof(WithModule)} method.");
            }

            AddModule(new WinUiNavigationModule(id, () => Activator.CreateInstance<TView>(), typeof(TView), () => Activator.CreateInstance<TDataContext>(), typeof(TDataContext), lifecycleOptions));
            return this;
        }

        public WinUiFrameNavigationService WithModule<TView, TDataContext>(string id, Func<TDataContext> dataContextBuilder)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
                => WithModule<TView, TDataContext>(id, dataContextBuilder, new NavigationModuleLifecycleOptions());

        public WinUiFrameNavigationService WithModule<TView, TDataContext>(string id, Func<TDataContext> dataContextBuilder, NavigationModuleLifecycleOptions lifecycleOptions)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
        {
            if (typeof(TView).GetConstructor(Type.EmptyTypes) is null)
            {
                throw new ArgumentException("Provided view type does not have parameterless constructor and cannot be registered " +
                    $"by this overload of {nameof(WithModule)} method.");
            }

            AddModule(new WinUiNavigationModule(id, () => Activator.CreateInstance<TView>(), typeof(TView), dataContextBuilder, typeof(TDataContext), lifecycleOptions));
            return this;
        }

        public WinUiFrameNavigationService WithModule<TView, TDataContext>(string id, Func<TView> viewBuilder, Func<TDataContext> dataContextBuilder)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
                => WithModule(id, viewBuilder, dataContextBuilder, new NavigationModuleLifecycleOptions());

        public WinUiFrameNavigationService WithModule<TView, TDataContext>(string id, Func<TView> viewBuilder, Func<TDataContext> dataContextBuilder, NavigationModuleLifecycleOptions lifecycleOptions)
            where TView : FrameworkElement
            where TDataContext : class, INavigatable
        {
            AddModule(new WinUiNavigationModule(id, viewBuilder, typeof(TView), dataContextBuilder, typeof(TDataContext), lifecycleOptions));
            return this;
        }

        public WinUiFrameNavigationService WithRoot(Window window)
        {
            INavigatable? dataContext = (window.Content as FrameworkElement)?.DataContext as INavigatable;
            if (dataContext == null)
            {
                throw new ArgumentException($"Cannot find data context of provided object or data context is not {nameof(INavigatable)}");
            }
            return WithRoot(window, dataContext);
        }

        public WinUiFrameNavigationService WithRoot(Window window, INavigatable dataContext)
        {
            AddModule(new WinUiNavigationModule(window, dataContext));
            return this;
        }

        public WinUiFrameNavigationService WithInitialId(string initialId)
        {
            SelectInitialtId(initialId);
            return this;
        }

        public WinUiFrameNavigationService WithFrame(Func<Frame> frameDelegate)
        {
            _frameDelegate = frameDelegate;
            return this;
        }

        public async Task<WinUiFrameNavigationService> Initialize()
        {
            await base.Initialize();
            return this;
        }

        protected override INavigationLifecycleModule HandleNavigationRequest(NavigationRequestData requestData)
        {
            if (_frameDelegate is null)
            {
                throw new InvalidOperationException($"Frame delegate must be specified to perform this action. Use {nameof(WithFrame)}() method first.");
            }

            INavigationLifecycleModule previousModule = _navigationStack.CurrentItem.Module;
            int previousIndex = NavigationHistory.CurrentIndex;

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

            INavigationLifecycleModule module = _navigationStack.CurrentItem.Module;

            if (requestData.NavigationType == NavigationType.Push)
            {
                _frameDelegate.Invoke().Navigate(module.ViewType);
            }

            if (requestData.NavigationType == NavigationType.Back)
            {
                for (int i = 0; i < requestData.Step; i++)
                {
                    _frameDelegate.Invoke().GoBack();
                }
            }

            if (requestData.NavigationType == NavigationType.Forward)
            {
                for (int i = 0; i < requestData.Step; i++)
                {
                    _frameDelegate.Invoke().GoForward();
                }
            }

            if (requestData.NavigationType == NavigationType.PushModal)
            {
                _frameDelegate.Invoke().Navigate(module.ViewType);
            }

            if (requestData.NavigationType == NavigationType.Pop)
            {
                int stepNumber = previousIndex - GetLastModalItemIndex();
                for (int i = 0; i < stepNumber; i++)
                {
                    _frameDelegate.Invoke().GoBack();
                }
            }
            
            if (!CurrentModule.IsInitialized)
            {
                module.Initialize(_frameDelegate.Invoke().Content);
                (module.DataContext?.Navigator as INavigatorInitialize)?.Initialize(module, this);
            }

            return previousModule;
        }
    }
}
