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
        /// An object which is initiating a navigation process.
        /// </summary>
        public object? Caller { get; }

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

        /// <summary>
        /// Determines whether cached module should be ignored while resolving navigation target.
        /// </summary>
        public bool IgnoreCached { get; }

        /// <summary>
        /// Determines whether module which is active during navigation request should be cached.
        /// This property overwrite <see cref="NavigationModuleLifecycleOptions.PreferCache"/> value.
        /// If this value is set to <see langword="null"/> only <see cref="NavigationModuleLifecycleOptions.PreferCache"/>
        /// will be considered.
        /// </summary>
        public bool? SaveCurrent { get; }

        /// <summary>
        /// Allows for defining custom navigation events behaviour.
        /// </summary>
        public NavigationEventsOptions? NavigationEventsOptions { get; }

        /// <summary>
        /// Creates a <see cref="NavigationType.Push"/> request with the provided parameters.
        /// </summary>
        /// <param name="targetId">The identifier of the target <see cref="INavigationModule"/>.</param>
        /// <param name="callingModule">An object that initiates the navigation transition.</param>
        /// <param name="parameter">
        /// An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in the navigation transition.
        /// </param>
        /// <param name="saveCurrent">
        /// Determines whether the module which is active during the navigation request should be cached.
        /// This property overwrites the <see cref="NavigationModuleLifecycleOptions.PreferCache"/> value.
        /// If this value is set to <see langword="null"/>, only the <see cref="NavigationModuleLifecycleOptions.PreferCache"/>
        /// value will be considered.
        /// </param>
        /// <param name="ignoreCached">
        /// Determines whether the cached module should be ignored while resolving the navigation target.
        /// </param>
        /// <param name="eventOptions">Allows for defining custom navigation events behavior.</param>
        public static NavigationRequestData CreatePushRequest(string targetId, object? callingModule, object? parameter, bool? saveCurrent, bool ignoreCached, NavigationEventsOptions? eventOptions)
                    => new(targetId, callingModule, parameter, NavigationType.Push, 0, saveCurrent, ignoreCached, eventOptions);

        /// <summary>
        /// Creates a <see cref="NavigationType.Back"/> request with the provided parameters.
        /// </summary>
        /// <param name="step">Describes how many layers of navigation should be traversed back.</param>
        /// <param name="callingModule">An object that initiates the navigation transition.</param>
        /// <param name="parameter">
        /// An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in the navigation transition.
        /// </param>
        /// <param name="saveCurrent">
        /// Determines whether the module which is active during the navigation request should be cached.
        /// This property overwrites the <see cref="NavigationModuleLifecycleOptions.PreferCache"/> value.
        /// If this value is set to <see langword="null"/>, only the <see cref="NavigationModuleLifecycleOptions.PreferCache"/>
        /// value will be considered.
        /// </param>
        /// <param name="ignoreCached">
        /// Determines whether the cached module should be ignored while resolving the navigation target.
        /// </param>
        /// <param name="eventOptions">Allows for defining custom navigation events behavior.</param>
        public static NavigationRequestData CreateBackRequest(int step, object? callingModule, object? parameter, bool? saveCurrent, bool ignoreCached, NavigationEventsOptions? eventOptions)
            => new(null, callingModule, parameter, NavigationType.Back, step, saveCurrent, ignoreCached, eventOptions);

        /// <summary>
        /// Creates a <see cref="NavigationType.Forward"/> request with the provided parameters.
        /// </summary>
        /// <param name="step">Describes how many layers of navigation should be traversed forward.</param>
        /// <param name="callingModule">An object that initiates the navigation transition.</param>
        /// <param name="parameter">
        /// An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in the navigation transition.
        /// </param>
        /// <param name="saveCurrent">
        /// Determines whether the module which is active during the navigation request should be cached.
        /// This property overwrites the <see cref="NavigationModuleLifecycleOptions.PreferCache"/> value.
        /// If this value is set to <see langword="null"/>, only the <see cref="NavigationModuleLifecycleOptions.PreferCache"/>
        /// value will be considered.
        /// </param>
        /// <param name="ignoreCached">
        /// Determines whether the cached module should be ignored while resolving the navigation target.
        /// </param>
        /// <param name="eventOptions">Allows for defining custom navigation events behavior.</param>
        public static NavigationRequestData CreateForwardRequest(int step, object? callingModule, object? parameter, bool? saveCurrent, bool ignoreCached, NavigationEventsOptions? eventOptions)
            => new(null, callingModule, parameter, NavigationType.Forward, step, saveCurrent, ignoreCached, eventOptions);

        /// <summary>
        /// Creates a <see cref="NavigationType.PushModal"/> request with the provided parameters.
        /// </summary>
        /// <param name="targetId">The identifier of the target <see cref="INavigationModule"/>.</param>
        /// <param name="callingModule">An object that initiates the navigation transition.</param>
        /// <param name="parameter">
        /// An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in the navigation transition.
        /// </param>
        /// <param name="ignoreCached">
        /// Determines whether the cached module should be ignored while resolving the navigation target.
        /// </param>
        /// <param name="eventOptions">Allows for defining custom navigation events behavior.</param>
        public static NavigationRequestData CreateModalRequest(string targetId, object? callingModule, object? parameter, bool ignoreCached, NavigationEventsOptions? eventOptions)
            => new(targetId, callingModule, parameter, NavigationType.PushModal, 0, true, ignoreCached, eventOptions);

        /// <summary>
        /// Creates a <see cref="NavigationType.Pop"/> request with the provided parameters.
        /// </summary>
        /// <param name="callingModule">An object that initiates the navigation transition.</param>
        /// <param name="returnValue">A Value to be returned from the modal request.</param>
        /// <param name="eventOptions">Allows for defining custom navigation events behavior.</param>
        public static NavigationRequestData CreatePopRequest(object? callingModule, object? returnValue, NavigationEventsOptions? eventOptions)
            => new(null, callingModule, returnValue, NavigationType.Pop, 0, false, false, eventOptions);

        protected NavigationRequestData
            (string? targetId,
            object? callingModule,
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
            Caller = callingModule;
            Parameter = parameter;
            NavigationType = navigationType;
            Step = step;
            IgnoreCached = ignoreCached;
            SaveCurrent = saveCurrent;
            NavigationEventsOptions = navigationEventsOptions;
        }
    }
}
