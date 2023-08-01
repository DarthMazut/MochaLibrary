using System;

namespace MochaCore.Navigation
{
    /// <summary>
    /// Provides arguments for implementations of <see cref="IOnNavigatedFrom"/> and <see cref="IOnNavigatedFromAsync"/>
    /// interfaces. 
    /// </summary>
    public class OnNavigatedFromEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnNavigatedFromEventArgs"/> class.
        /// </summary>
        /// <param name="caller">An object which is initiating a navigation process.</param>
        /// <param name="currentModule">Currently active <see cref="INavigationModule"/>.</param>
        /// <param name="parameter">An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in navigation transition.</param>
        public OnNavigatedFromEventArgs(object? caller, INavigationModule currentModule, object? parameter)
            : this (caller, currentModule, parameter, NavigationType.Push, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OnNavigatedFromEventArgs"/> class.
        /// </summary>
        /// <param name="caller">An object which is initiating a navigation process.</param>
        /// <param name="currentModule">Currently active <see cref="INavigationModule"/>.</param>
        /// <param name="parameter">An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in navigation transition.</param>
        /// <param name="navigationType">Defines type of navigation process.</param>
        /// <param name="step">
        /// If <see cref="NavigationType"/> is <see cref="NavigationType.Back"/> or <see cref="NavigationType.Forward"/>
        /// it describes how many layers of navigation should be traversed in the indicated direction. 
        /// This property is ignored for other navigation types.
        /// </param>
        public OnNavigatedFromEventArgs(
            object? caller,
            INavigationModule currentModule,
            object? parameter,
            NavigationType navigationType,
            int step)
        {
            Caller = caller;
            CurrentModule = currentModule ?? throw new ArgumentNullException(nameof(currentModule));
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
        /// Currently active <see cref="INavigationModule"/>.
        /// </summary>
        public INavigationModule CurrentModule { get; }

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