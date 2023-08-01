using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Navigation
{
    /// <summary>
    /// Provides arguments for <see cref="INavigationService.CurrentModuleChanged"/> event.
    /// </summary>
    public class CurrentNavigationModuleChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentNavigationModuleChangedEventArgs"/> class.
        /// </summary>
        /// <param name="currentModule">
        /// Currently active module at the time <see cref="INavigationService.CurrentModuleChanged"/> was invoked.
        /// </param>
        /// <param name="caller">An object which is initiating a navigation process.</param>
        /// <param name="previousModule">Active <see cref="INavigationModule"/> at the time navigation was requested.</param>
        /// <param name="parameter">
        /// An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in navigation transition.
        /// </param>
        /// <param name="navigationType">Defines type of navigation process.</param>
        /// <param name="step">
        /// If <see cref="NavigationType"/> is <see cref="NavigationType.Back"/> or <see cref="NavigationType.Forward"/>
        /// it describes how many layers of navigation should be traversed in the indicated direction. 
        /// This property can be ignored in case <see cref="NavigationType"/> is set to <see cref="NavigationType.Push"/>, 
        /// <see cref="NavigationType.PushModal"/> or <see cref="NavigationType.Pop"/>.
        /// </param>
        public CurrentNavigationModuleChangedEventArgs(
            INavigationModule currentModule,
            object? caller,
            INavigationModule? previousModule,
            object? parameter,
            NavigationType navigationType,
            int step)
        {
            CurrentModule = currentModule;
            Caller = caller;
            PreviousModule = previousModule;
            Parameter = parameter;
            NavigationType = navigationType;
            Step = step;
        }

        public static CurrentNavigationModuleChangedEventArgs FromNavigatedToEventArgs(OnNavigatedToEventArgs navigatedToArgs, INavigationModule currentModule)
            => new CurrentNavigationModuleChangedEventArgs(currentModule, navigatedToArgs.Caller, navigatedToArgs.PreviousModule, navigatedToArgs.Parameter, navigatedToArgs.NavigationType, navigatedToArgs.Step);

        /// <summary>
        /// Currently active module of corresponding <see cref="INavigationService"/> instance.
        /// </summary>
        public INavigationModule CurrentModule { get; }

        /// <summary>
        /// An object which is initiating a navigation process.
        /// </summary>
        public object? Caller { get; }

        /// <summary>
        /// An <see cref="INavigationModule"/> object which is initiating a navigation process.
        /// </summary>
        public INavigationModule? CallingModule => Caller as INavigationModule;

        /// <summary>
        /// Active <see cref="INavigationModule"/> at the time navigation was requested.
        /// </summary>
        public INavigationModule? PreviousModule { get; }

        /// <summary>
        /// An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in navigation transition.
        /// </summary>
        public object? Parameter { get; }

        /// <summary>
        /// Defines type of navigation process.
        /// </summary>
        public NavigationType NavigationType { get; }

        /// <summary>
        /// If <see cref="NavigationType"/> is <see cref="NavigationType.Back"/> or <see cref="NavigationType.Forward"/>
        /// it describes how many layers of navigation should be traversed in the indicated direction. 
        /// This property can be ignored in case <see cref="NavigationType"/> is set to <see cref="NavigationType.Push"/>, 
        /// <see cref="NavigationType.PushModal"/> or <see cref="NavigationType.Pop"/>.
        /// </summary>
        public int Step { get; }
    }
}
