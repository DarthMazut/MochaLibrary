using System;
using System.ComponentModel;

namespace MochaCore.Navigation
{
    /// <summary>
    /// Provides arguments for implementations of <see cref="IOnNavigatingFrom"/> and <see cref="IOnNavigatingFromAsync"/>
    /// interfaces.
    /// </summary>
    public class OnNavigatingFromEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnNavigatingFromEventArgs"/> class.
        /// </summary>
        /// <param name="caller">An object which is initiating a navigation process.</param>
        /// <param name="requestedModule"> An <see cref="INavigationModule"/> object representing navigation target.</param>
        /// <param name="parameter">An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in navigation transition.</param>
        public OnNavigatingFromEventArgs(object? caller, INavigationModule requestedModule, object? parameter)
            : this(caller, requestedModule, parameter, NavigationType.Push, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OnNavigatingFromEventArgs"/> class.
        /// </summary>
        /// <param name="caller">An object which is initiating a navigation process.</param>
        /// <param name="requestedModule"> An <see cref="INavigationModule"/> object representing navigation target.</param>
        /// <param name="parameter">An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in navigation transition.</param>
        /// <param name="navigationType">Defines type of navigation process.</param>
        /// <param name="step">
        /// If <see cref="NavigationType"/> is <see cref="NavigationType.Back"/> or <see cref="NavigationType.Forward"/>
        /// it describes how many layers of navigation should be traversed in the indicated direction. 
        /// This property is ignored for other navigation types.
        /// </param>
        public OnNavigatingFromEventArgs(
            object? caller,
            INavigationModule requestedModule,
            object? parameter,
            NavigationType navigationType,
            int step)
        {
            Caller = caller;
            RequestedModule = requestedModule ?? throw new ArgumentNullException(nameof(requestedModule));
            Parameter = parameter;
            NavigationType = navigationType;
            Step = step;
        }

        /// <summary>
        /// An object which is initiating a navigation process.
        /// </summary>
        public object? Caller { get; }

        /// <summary>
        /// An <see cref="INavigationModule"/> object which is initiating a navigation process.
        /// </summary>
        public INavigationModule? CallingModule => Caller as INavigationModule;

        /// <summary>
        /// An <see cref="INavigationModule"/> object representing navigation target.
        /// </summary>
        public INavigationModule RequestedModule { get; }

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
        /// This property is ignored for other navigation types.
        /// </summary>
        public int Step { get; }
    }
}