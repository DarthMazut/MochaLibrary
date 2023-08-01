namespace MochaCore.Navigation
{
    /// <summary>
    /// Allows an implementing class to expose an <c>OnNavigatedTo()</c> method 
    /// that is called by <see cref="INavigationService"/> whenever 
    /// the navigation process is about to finish, and the target <see cref="INavigationModule"/>
    /// is already displayed.
    /// </summary>
    public interface IOnNavigatedTo
    {
        /// <summary>
        /// Called when the navigation process is about to finish and a new <see cref="INavigationModule"/> 
        /// is displayed. This event should contain setup code for the current <see cref="INavigationParticipant"/>,
        /// including event subscription. At this point, the navigation process cannot be rejected. 
        /// </summary>
        /// <param name="e">Provides navigation request details.</param>
        public void OnNavigatedTo(OnNavigatedToEventArgs e);
    }
}
