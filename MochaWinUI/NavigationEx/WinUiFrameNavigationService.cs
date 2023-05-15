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
        private NavigationRequestData? _requestData;

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

        protected override void InitializeModule(INavigationLifecycleModule module)
        {
            if (_frameDelegate is null)
            {
                throw new InvalidOperationException($"Frame delegate must be specified to perform this action. Use {nameof(WithFrame)}() method first.");
            }

            Frame frame = _frameDelegate.Invoke();

            if (_requestData is null)
            {
                frame.Navigate(module.ViewType);
                module.Initialize(frame.Content);
                (module.DataContext?.Navigator as INavigatorInitialize)?.Initialize(module, this);
            }
            else
            {
                if (_requestData.NavigationType == NavigationType.Push)
                {
                    frame.Navigate(module.ViewType);
                }

                if (_requestData.NavigationType == NavigationType.Back)
                {
                    for (int i = 0; i < _requestData.Step; i++)
                    {
                        frame.GoBack();
                    }
                }

                if (_requestData.NavigationType == NavigationType.Forward)
                {
                    for (int i = 0; i < _requestData.Step; i++)
                    {
                        frame.GoForward();
                    }
                }

                if (_requestData.NavigationType == NavigationType.PushModal)
                {
                    frame.Navigate(module.ViewType);
                }

                if (_requestData.NavigationType == NavigationType.Pop)
                {
                    int stepNumber = 1; //previousIndex - GetLastModalItemIndex();
                    for (int i = 0; i < stepNumber; i++)
                    {
                        frame.GoBack();
                    }
                }

                if (CurrentModule.IsInitialized)
                {
                    if (CurrentModule.View == frame.Content)
                    {
                        return;
                    }
                    else
                    {
                        _navigationStack.CurrentItem.Module.Uninitialize();
                    }
                }

                module.Initialize(frame.Content);
                (module.DataContext?.Navigator as INavigatorInitialize)?.Initialize(module, this);
            }  
        }

        protected override INavigationLifecycleModule HandleNavigationRequest(NavigationRequestData requestData)
        {
            _requestData = requestData;
            return base.HandleNavigationRequest(requestData);
        }
    }
}
