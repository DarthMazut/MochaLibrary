using System;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Provides arguments for implementations of <see cref="IOnNavigatedTo"/> and <see cref="IOnNavigatedToAsync"/>
    /// interfaces. 
    /// </summary>
    public class OnNavigatedToEventArgs : EventArgs
    {
        public OnNavigatedToEventArgs(INavigationModule? callingModule, INavigationModule? previousModule, object? parameter)
            : this(callingModule, previousModule, parameter, NavigationType.Push, 0) { }

        public OnNavigatedToEventArgs(
            INavigationModule? callingModule,
            INavigationModule? previousModule,
            object? parameter,
            NavigationType navigationType,
            int step)
        {
            CallingModule = callingModule;
            PreviousModule = previousModule;
            Parameter = parameter;
            NavigationType = navigationType;
            Step = step;
        }


        /// <summary>
        /// An <see cref="INavigationModule"/> object which is initiating a navigation process.
        /// </summary>
        public INavigationModule? CallingModule { get; }

        /// <summary>
        /// Active <see cref="INavigationModule"/> at the time navigation was requested.
        /// </summary>
        public INavigationModule? PreviousModule { get; }

        /// <summary>
        /// An extra data object used to pass information between <see cref="INavigatable"/>
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