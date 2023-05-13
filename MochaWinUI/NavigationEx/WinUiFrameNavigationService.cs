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
        private Frame? _frame;

        /// <summary>
        /// Specifies the <see cref="Frame"/> object associated with this service.
        /// </summary>
        /// <param name="frame"><see cref="Frame"/> to be used by this service.</param>
        public void SetFrame(Frame frame) => _frame = frame;

        /// <inheritdoc/>
        protected override void InitializeModule(INavigationLifecycleModule module)
        {
            if (_frame is null)
            {
                throw new InvalidOperationException($"Frame must be specified to perform this action. Use {nameof(SetFrame)} method first.");
            }

            module.Initialize(_frame.Content);
            (module.DataContext?.Navigator as INavigatorInitialize)?.Initialize(module, this);
        }

        /// <inheritdoc/>
        protected override void OnCurrentModuleChanged(CurrentNavigationModuleChangedEventArgs args)
        {
            base.OnCurrentModuleChanged(args);
        }
    }
}
