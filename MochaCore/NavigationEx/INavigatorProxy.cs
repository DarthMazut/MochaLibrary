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
        bool? SaveCurrent { get; set; }
        INavigationService Service { get; }

        /// <summary>
        /// Provides fluent API for building navigation requests.
        /// </summary>
        /// <param name="buildingDelegate">A delegate that is used to build navigation requests.</param>
        Task<NavigationResultData> NavigateAsync(Func<INavigationDestinationBuilder, INavigationRequestDetailsBuilder> buildingDelegate);
        Task<NavigationResultData> NavigateAsync(NavigationRequestData navigationRequestData);
        Task<NavigationResultData> NavigateAsync(string targetId);
        Task<NavigationResultData> NavigateAsync(string targetId, object? parameter);
        Task<NavigationResultData> NavigateBackAsync();
        Task<NavigationResultData> NavigateBackAsync(object? parameter);
        Task<NavigationResultData> NavigateBackAsync(object? parameter, int step);
        Task<NavigationResultData> NavigateForwardAsync();
        Task<NavigationResultData> NavigateForwardAsync(object? parameter);
        Task<NavigationResultData> NavigateForwardAsync(object? parameter, int step);
        Task<NavigationResultData> NavigateModalAsync(string targetId);
        Task<NavigationResultData> NavigateModalAsync(string targetId, object? parameter);
    }
}