using System;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Exposes API for navigation.
    /// </summary>
    public interface INavigator : INavigatorProxy
    {
        /// <summary>
        /// Determines whether it is currently possible to return to the module that initiated the last modal request.
        /// </summary>
        bool CanReturnModal { get; }

        /// <summary>
        /// Provides latest navigation context.
        /// </summary>
        OnNavigatedToEventArgs? Context { get; }
        
        /// <summary>
        /// Determines whether this instance is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// An <see cref="INavigationModule"/> associated with current instance.
        /// </summary>
        INavigationModule Module { get; }

        /// <summary>
        /// Occurs when ...
        /// </summary>
        event EventHandler<NavigatorInitializedEventArgs>? Initialized;

        Task ReturnModal();
        Task ReturnModal(object returnData);
        Task ReturnModal(object? returnData, NavigationEventsOptions? eventsOptions);
    }
}