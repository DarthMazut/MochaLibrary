using System;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Provides arguments for implementations of <see cref="IOnNavigatedFrom"/> and <see cref="IOnNavigatedFromAsync"/>
    /// interfaces. 
    /// </summary>
    public class OnNavigatedFromEventArgs
    {
        public OnNavigatedFromEventArgs(object? caller, INavigationModule currentModule, object? parameter)
            : this (caller, currentModule, parameter, NavigationType.Push, 0) { }

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

        public object? Caller { get; }

        public INavigationModule? CallingModule => Caller as INavigationModule;

        public INavigationModule CurrentModule { get; }

        public object? Parameter { get; }

        public NavigationType NavigationType { get; }

        public int Step { get; }
    }
}