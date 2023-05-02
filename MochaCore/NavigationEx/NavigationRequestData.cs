using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Contains a data required for performing navigation.
    /// </summary>
    public class NavigationRequestData
    {
        /// <summary>
        /// Unique indentifier of target <see cref="INavigationModule"/>
        /// </summary>
        public string? TargetId { get; }

        /// <summary>
        /// An <see cref="INavigationModule"/> object which is initiating a navigation process.
        /// </summary>
        public INavigationModule CallingModule { get; }

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

        /// <summary>
        /// Determines whether cached module should be ignored while resolving navigation target.
        /// </summary>
        public bool IgnoreCached { get; }

        /// <summary>
        /// Determines whether module which is active during navigation request should be cached.
        /// This property overwrite <see cref="INavigationLifecycleModule.PreferCache"/> value.
        /// If this value is set to <see langword="null"/> only <see cref="INavigationLifecycleModule.PreferCache"/>
        /// will be considered.
        /// </summary>
        public bool? SaveCurrent { get; }

        /// <summary>
        /// Allows for defining custom navigation events behaviour.
        /// </summary>
        public NavigationEventsOptions? NavigationEventsOptions { get; }

        public NavigationRequestData(string targetId, INavigationModule callingModule, object? parameter, bool? saveCurrent, bool ignoreCached)
            : this(targetId, callingModule, parameter, NavigationType.Push, 0, saveCurrent, ignoreCached, null) { }

        public NavigationRequestData(NavigationType navigationType, int step, INavigationModule callingModule, object? parameter, bool? saveCurrent, bool ignoreCached)
            : this(null, callingModule, parameter, navigationType, step, saveCurrent, ignoreCached, null) { }

        public static NavigationRequestData CreateModalRequest(string targetId, INavigationModule callingModule, object? parameter, NavigationEventsOptions? eventsOptions)
            => new NavigationRequestData(targetId, callingModule, parameter, NavigationType.PushModal, 0, true, false, eventsOptions);

        public static NavigationRequestData CreatePopRequest(INavigationModule callingModule, object? returnValue, NavigationEventsOptions eventsOptions)
            => new NavigationRequestData(null, callingModule, returnValue, NavigationType.Pop, 0, false, false, eventsOptions);

        private NavigationRequestData
            (string? targetId,
            INavigationModule callingModule,
            object? parameter,
            NavigationType navigationType,
            int step,
            bool? saveCurrent,
            bool ignoreCached,
            NavigationEventsOptions? navigationEventsOptions)
        {
            if (navigationType == NavigationType.Back || navigationType == NavigationType.Forward)
            {
                if (step <= 0)
                {
                    throw new ArgumentException("Step value must be greater than 0", nameof(step));
                }
            }

            TargetId = targetId;
            CallingModule = callingModule;
            Parameter = parameter;
            NavigationType = navigationType;
            Step = step;
            IgnoreCached = ignoreCached;
            SaveCurrent = saveCurrent;
            NavigationEventsOptions = navigationEventsOptions;
        }
    }
}
