namespace MochaCore.Navigation
{
    /// <summary>
    /// Provides an abstraction for supplying navigation context to <see cref="INavigator"/> instances.
    /// By implementing this interface explicitly, you can hide the <see cref="SetNavigationContext(OnNavigatedToEventArgs)"/>
    /// method from the client who is not supposed to call it directly.
    /// </summary>
    public interface ISetNavigationContext
    {
        /// <summary>
        /// Sets the navigation context on the <see cref="INavigator"/> instance.
        /// </summary>
        /// <param name="context">The navigation context to be set.</param>
        void SetNavigationContext(OnNavigatedToEventArgs context);
    }
}