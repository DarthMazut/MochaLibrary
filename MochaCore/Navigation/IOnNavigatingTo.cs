namespace MochaCore.Navigation
{
    /// <summary>
    /// Allows implementing class to expose an <c>OnNavigatingTo()</c> method 
    /// which is called by <see cref="NavigationService"/> whenever 
    /// navigation is requested to parent <see cref="INavigationModule"/>.
    /// </summary>
    public interface IOnNavigatingTo
    {
        /// <summary>
        /// Called whenever any <see cref="INavigatable"/> requested a navigation to <see cref="INavigationModule"/> that
        /// holds this <see cref="INavigatable"/> instance. Do not put any set up code here including event subscribtion as 
        /// navigation can be rejected at this point. Use this event for deciding whether to reject navigation process.
        /// </summary>
        /// <param name="navigationData">Details of navigation request.</param>
        /// <param name="e">Allows for rejecting current navigation process.</param>
        void OnNavigatingTo(NavigationData navigationData, NavigationCancelEventArgs e);
    }
}
