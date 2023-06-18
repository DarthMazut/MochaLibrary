using System;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Exposes API for proxy navigation.
    /// </summary>
    public interface INavigatorProxy
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
        /// An <see cref="INavigationService"/> associated with curren <see cref="INavigatorProxy"/> instance.
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
        /// An extra data object used to pass information between <see cref="INavigatable"/>
        /// objects that take part in navigation transition.
        /// </param>
        Task<NavigationResultData> NavigateAsync(string targetId, object? parameter);

        /// <summary>
        /// Navigates back to the preceding element on the navigation stack.
        /// </summary>
        /// <remarks>
        /// This method doesn't pop any element from the navigation stack; 
        /// it only moves the pointer of the current element to the preceding element.
        /// </remarks>
        Task<NavigationResultData> NavigateBackAsync();

        /// <summary>
        /// Navigates back to the preceding element on the navigation stack.
        /// </summary>
        /// <remarks>
        /// This method doesn't pop any element from the navigation stack; 
        /// it only moves the pointer of the current element to the preceding element.
        /// </remarks>
        /// <param name="parameter">
        /// An extra data object used to pass information between <see cref="INavigatable"/>
        /// objects that take part in navigation transition.
        /// </param>
        Task<NavigationResultData> NavigateBackAsync(object? parameter);

        /// <summary>
        /// Navigates back to the preceding element on the navigation stack.
        /// </summary>
        /// <remarks>
        /// This method doesn't pop any element from the navigation stack; 
        /// it only moves the pointer of the current element to the preceding element.
        /// </remarks>
        /// <param name="parameter">
        /// An extra data object used to pass information between <see cref="INavigatable"/>
        /// objects that take part in navigation transition.
        /// </param>
        /// <param name="step">
        /// Determines by how many elements the pointer of the current navigation element will be shifted back.
        /// </param>
        Task<NavigationResultData> NavigateBackAsync(object? parameter, int step);

        Task<NavigationResultData> NavigateForwardAsync();
        Task<NavigationResultData> NavigateForwardAsync(object? parameter);
        Task<NavigationResultData> NavigateForwardAsync(object? parameter, int step);
        Task<NavigationResultData> NavigateModalAsync(string targetId);
        Task<NavigationResultData> NavigateModalAsync(string targetId, object? parameter);
    }
}