using System;
using System.ComponentModel;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Provides arguments for implementations of <see cref="IOnNavigatingFrom"/> and <see cref="IOnNavigatingFromAsync"/>
    /// interfaces.
    /// </summary>
    public class OnNavigatingFromEventArgs : CancelEventArgs
    {
        public OnNavigatingFromEventArgs(object? caller, INavigationModule requestedModule, object? parameter)
            : this(caller, requestedModule, parameter, NavigationType.Push, 0) { }

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
        /// This property is ignored in case  <see cref="NavigationType"/> is set to <see cref="NavigationType.Push"/>.
        /// </summary>
        public int Step { get; }
    }
}