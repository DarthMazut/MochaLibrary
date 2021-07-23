namespace MochaCore.Navigation
{
    /// <summary>
    /// Allows implementing class to expose an <c>OnNavigatedTo()</c> method 
    /// which is called by <see cref="NavigationService"/> whenever 
    /// navigation process is about to finish.
    /// </summary>
    public interface IOnNavigatedTo
    {
        /// <summary>
        /// Called when navigation process is about to finish and new view is displayed.
        /// This event should contain a set up code for current <see cref="INavigatable"/>
        /// including event subscribtion. At this point navigation process cannot be rejected. 
        /// </summary>
        /// <param name="navigationData">Details of navigation request.</param>
        void OnNavigatedTo(NavigationData navigationData);
    }
}
