using System;
using System.Threading.Tasks;

namespace MochaCore.Navigation
{
    /// <summary>
    /// Exposes API for navigation.
    /// </summary>
    public interface INavigator : IRemoteNavigator
    {
        /// <summary>
        /// Determines whether it is currently possible to return to the <see cref="INavigationModule"/> 
        /// that initiated the last <see cref="NavigationType.PushModal"/> request.
        /// </summary>
        bool CanReturnModal { get; }

        /// <summary>
        /// Provides latest navigation context.
        /// </summary>
        OnNavigatedToEventArgs? Context { get; }
        
        /// <summary>
        /// Determines whether this instance is initialized. <see cref="INavigator"/> implementations
        /// are initialized by <see cref="INavigationService"/> instance
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// An <see cref="INavigationModule"/> associated with current instance.
        /// </summary>
        INavigationModule Module { get; }

        /// <summary>
        /// Occurs when current instance is initialized.
        /// </summary>
        event EventHandler<NavigatorInitializedEventArgs>? Initialized;

        /// <summary>
        /// Finishes the most recent <see cref="NavigationType.PushModal"/> navigation request
        /// by navigating back to its origin <see cref="INavigationModule"/> with <see langword="null"/>
        /// as the return value. Throws an exception if returning from modal navigation is currently not possible.
        /// You can inspect the <see cref="CanReturnModal"/> property to determine whether this method will throw.
        /// </summary>
        Task ReturnModal();

        /// <summary>
        /// Finishes the most recent <see cref="NavigationType.PushModal"/> navigation request
        /// by navigating back to its origin <see cref="INavigationModule"/> with the specified return data.
        /// Throws an exception if returning from modal navigation is currently not possible.
        /// You can inspect the <see cref="CanReturnModal"/> property to determine whether this method will throw.
        /// </summary>
        /// <param name="returnData">The data to be returned from the modal navigation.</param>
        Task ReturnModal(object returnData);

        /// <summary>
        /// Finishes the most recent <see cref="NavigationType.PushModal"/> navigation request
        /// by navigating back to its origin <see cref="INavigationModule"/> with the specified return data.
        /// Throws an exception if returning from modal navigation is currently not possible.
        /// You can inspect the <see cref="CanReturnModal"/> property to determine whether this method will throw.
        /// </summary>
        /// <param name="returnData">The data to be returned from the modal navigation.</param>
        /// <param name="eventsOptions">Allows for defining custom navigation events behaviour.</param>
        Task ReturnModal(object? returnData, NavigationEventsOptions? eventsOptions);
    }
}