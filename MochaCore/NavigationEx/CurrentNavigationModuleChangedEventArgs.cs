using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Provides arguments for <see cref="INavigationService.CurrentModuleChanged"/> event.
    /// </summary>
    public class CurrentNavigationModuleChangedEventArgs : OnNavigatedToEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentNavigationModuleChangedEventArgs"/> class.
        /// </summary>
        /// <param name="module">
        /// Currently active module at the time <see cref="INavigationService.CurrentModuleChanged"/> was invoked.
        /// </param>
        /// <param name="navigatedToArgs">An <see cref="OnNavigatedToEventArgs"/> instance matching creating object.</param>
        public CurrentNavigationModuleChangedEventArgs(INavigationModule module, OnNavigatedToEventArgs navigatedToArgs)
            : base(
              navigatedToArgs.CallingModule,
              navigatedToArgs.PreviousModule,
              navigatedToArgs.Parameter,
              navigatedToArgs.NavigationType,
              navigatedToArgs.Step)
        {
            CurrentModule = module;
        }

        /// <summary>
        /// Currently active module of corresponding <see cref="INavigationService"/> instance.
        /// </summary>
        public INavigationModule CurrentModule { get; }
    }
}
