using System;

namespace MochaCore.NavigationEx
{
    public class OnNavigatedFromEventArgs
    {
        public OnNavigatedFromEventArgs(INavigationModule callingModule, INavigationModule currentModule, object? parameter)
            : this (callingModule, currentModule, parameter, NavigationType.Push, 0) { }

        public OnNavigatedFromEventArgs(
            INavigationModule callingModule,
            INavigationModule currentModule,
            object? parameter,
            NavigationType navigationType,
            int step)
        {
            CallingModule = callingModule ?? throw new ArgumentNullException(nameof(callingModule));
            CurrentModule = currentModule ?? throw new ArgumentNullException(nameof(currentModule));
            Parameter = parameter;
            NavigationType = navigationType;
            Step = step;
        }

        public INavigationModule CallingModule { get; }

        public INavigationModule CurrentModule { get; }

        public object? Parameter { get; }

        public NavigationType NavigationType { get; }

        public int Step { get; }
    }
}