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
    public class WinUiFrameNavigationService : WinUiNavigationService
    {
        private Frame? _frame;
        private NavigationRequestData? _currentData;

        /// <summary>
        /// Specifies the <see cref="Frame"/> object associated with this service.
        /// </summary>
        /// <param name="frame"><see cref="Frame"/> to be used by this service.</param>
        public void SetFrame(Frame frame) => _frame = frame;

        public WinUiFrameNavigationService WithFrame(Frame frame)
        {
            _frame = frame;
            return this;
        }

        protected override INavigationLifecycleModule HandleNavigationRequest(NavigationRequestData requestData)
        {
            _currentData = requestData;
            return base.HandleNavigationRequest(requestData);
        }

        /// <inheritdoc/>
        protected override void InitializeModule(INavigationLifecycleModule module)
        {
            if (_frame is null)
            {
                throw new InvalidOperationException($"Frame must be specified to perform this action. Use {nameof(SetFrame)} method first.");
            }

            if (_currentData is null)
            {
                _frame.Navigate(module.ViewType);
            }
            else
            {
                if (_currentData.NavigationType == NavigationType.Push)
                {
                    _frame.Navigate(module.ViewType);
                }

                if (_currentData.NavigationType == NavigationType.Back)
                {
                    for (int i = 0; i < _currentData.Step; i++)
                    {
                        _frame.GoBack();
                    }
                }

                if (_currentData.NavigationType == NavigationType.Forward)
                {
                    for (int i = 0; i < _currentData.Step; i++)
                    {
                        _frame.GoForward();
                    }
                }

                if (_currentData.NavigationType == NavigationType.PushModal)
                {
                    _frame.Navigate(module.ViewType);
                }

                if (_currentData.NavigationType == NavigationType.Pop)
                {
                    int stepNumber = NavigationHistory.CurrentIndex - GetLastModalItemIndex();
                    for (int i = 0; i < stepNumber; i++)
                    {
                        _frame.GoBack();
                    }
                }
            }

            module.Initialize(_frame.Content);
            (module.DataContext?.Navigator as INavigatorInitialize)?.Initialize(module, this);
            _currentData = null;
        }
    }
}
