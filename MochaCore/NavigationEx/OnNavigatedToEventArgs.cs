using System;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Provides arguments for implementations of <see cref="IOnNavigatedTo"/> and <see cref="IOnNavigatedToAsync"/>
    /// interfaces. 
    /// </summary>
    public class OnNavigatedToEventArgs : EventArgs
    {
        public OnNavigatedToEventArgs(object? caller, INavigationModule? previousModule, object? parameter)
            : this(caller, previousModule, parameter, NavigationType.Push, 0) { }

        public OnNavigatedToEventArgs(
            object? caller,
            INavigationModule? previousModule,
            object? parameter,
            NavigationType navigationType,
            int step)
        {
            Caller = caller;
            PreviousModule = previousModule;
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