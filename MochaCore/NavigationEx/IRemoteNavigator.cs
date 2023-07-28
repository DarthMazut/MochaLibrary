using System;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Exposes API for remote navigation.
    /// </summary>
    public interface IRemoteNavigator
    {
        /// <summary>
        /// Determines whether backward navigation is available.
        /// </summary>
        bool CanNavigateBack { get; }

        /// <summary>
        /// Determines whether forward navigation is available.
        /// </summary>
        bool CanNavigateForward { get; }

        /// <summary>
        /// An object that claims to own the current instance. 
        /// </summary>
        object? Owner { get; }

        /// <summary>
        /// Determines whether module which is active during navigation request should be cached.
        /// This property overwrite <see cref="NavigationModuleLifecycleOptions.PreferCache"/> value.
        /// If this value is set to <see langword="null"/> only <see cref="NavigationModuleLifecycleOptions.PreferCache"/>
        /// will be considered.
        /// </summary>
        bool? SaveCurrent { get; set; }

        /// <summary>
        /// An <see cref="INavigationService"/> associated with current <see cref="IRemoteNavigator"/> instance.
        /// All <c>Navigate*Async</c> methods are called for that particular instance.
        /// </summary>
        INavigationService Service { get; }

        /// <summary>
        /// Provides fluent API for building navigation requests.
        /// </summary>
        /// <param name="buildingDelegate">A delegate that is used to build navigation requests.</param>
        Task<NavigationResultData> NavigateAsync(Func<INavigationDestinationBuilder, INavigationRequestDetailsBuilder> buildingDelegate);

        /// <summary>
        /// Sends a request to the associated <see cref="INavigationService"/> to initiate the navigation process.
        /// </summary>
        /// <param name="navigationRequestData">Contains a data required for performing navigation.</param>
        Task<NavigationResultData> NavigateAsync(NavigationRequestData navigationRequestData);

        /// <summary>
        /// Sends a request to the associated <see cref="INavigationService"/> to initiate the navigation process.
        /// </summary>
        /// <param name="targetId">Identifier of target <see cref="INavigationModule"/>.</param>
        Task<NavigationResultData> NavigateAsync(string targetId);

        /// <summary>
        /// Sends a request to the associated <see cref="INavigationService"/> to initiate the navigation process.
        /// </summary>
        /// <param name="targetId">Identifier of target <see cref="INavigationModule"/>.</param>
        /// <param name="parameter">
        /// An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in navigation transition.
        /// </param>
        Task<NavigationResultData> NavigateAsync(string targetId, object? parameter);

        /// <summary>
        /// Navigates back to the preceding element in the navigation stack.
        /// Throws if this operation is invalid for current state.
        /// <para>
        /// This method doesn't remove any element from the navigation stack;
        /// it only moves the pointer from the current element to the preceding element.
        /// </para>
        /// </summary>
        Task<NavigationResultData> NavigateBackAsync();

        /// <summary>
        /// Navigates back to the preceding element on the navigation stack.
        /// Throws if this operation is invalid for current state.
        /// <para>
        /// This method doesn't pop any element from the navigation stack; 
        /// it only moves the pointer of the current element to the preceding element.
        /// </para>
        /// </summary>
        /// <param name="parameter">
        /// An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in navigation transition.
        /// </param>
        Task<NavigationResultData> NavigateBackAsync(object? parameter);

        /// <summary>
        /// Navigates back to the preceding element on the navigation stack.
        /// Throws if this operation is invalid for current state.
        /// <para>
        /// This method doesn't pop any element from the navigation stack; 
        /// it only moves the pointer of the current element to the preceding element.
        /// </para>
        /// </summary>
        /// <param name="parameter">
        /// An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in navigation transition.
        /// </param>
        /// <param name="step">
        /// Determines by how many elements the pointer of the current navigation element will be shifted back.
        /// </param>
        Task<NavigationResultData> NavigateBackAsync(object? parameter, int step);

        /// <summary>
        /// Navigates forward to the subsequent element in the navigation stack.
        /// Throws if navigating forward is impossible at the time.
        /// <para>
        /// This method doesn't add any element to the navigation stack;
        /// it only moves the pointer from the current element to the subsequent element.
        /// </para>
        /// </summary>
        Task<NavigationResultData> NavigateForwardAsync();

        /// <summary>
        /// Navigates forward to the subsequent element in the navigation stack.
        /// Throws if navigating forward is impossible at the time.
        /// <para>
        /// This method doesn't add any element to the navigation stack;
        /// it only moves the pointer from the current element to the subsequent element.
        /// </para>
        /// </summary>
        /// <param name="parameter">
        /// An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in navigation transition.
        /// </param>
        Task<NavigationResultData> NavigateForwardAsync(object? parameter);

        /// <summary>
        /// Navigates forward to the subsequent element in the navigation stack.
        /// Throws if navigating forward is impossible at the time.
        /// <para>
        /// This method doesn't add any element to the navigation stack;
        /// it only moves the pointer from the current element to the subsequent element.
        /// </para>
        /// </summary>
        /// <param name="parameter">
        /// An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in navigation transition.
        /// </param>
        /// <param name="step">
        /// Determines by how many elements the pointer of the current navigation element will be shifted forward.
        /// </param>
        Task<NavigationResultData> NavigateForwardAsync(object? parameter, int step);
        
        /// <summary>
        /// Performs <see cref="NavigationType.PushModal"/> navigation to the specified <see cref="INavigationModule"/>.
        /// This type of navigation esures that the current module will not be disposed before returning
        /// from modal navigation.
        /// </summary>
        /// <param name="targetId">Identifier of target <see cref="INavigationModule"/>.</param>
        Task<object?> NavigateModalAsync(string targetId);

        /// <summary>
        /// Performs <see cref="NavigationType.PushModal"/> navigation to the specified <see cref="INavigationModule"/>.
        /// This type of navigation esures that the current module will not be disposed before returning
        /// from modal navigation.
        /// </summary>
        /// <param name="targetId">Identifier of target <see cref="INavigationModule"/>.</param>
        /// <param name="parameter">
        /// An extra data object used to pass information between <see cref="INavigationParticipant"/>
        /// objects that take part in navigation transition.
        /// </param>
        Task<object?> NavigateModalAsync(string targetId, object? parameter);
    }
}