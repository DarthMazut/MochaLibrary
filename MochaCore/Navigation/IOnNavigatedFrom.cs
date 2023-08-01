namespace MochaCore.Navigation
{

    /// <summary>
    /// Allows an implementing class to expose the <c>OnNavigatedFrom()</c> method, 
    /// which is called by <see cref="INavigationService"/> whenever the navigation process
    /// is about to complete.
    /// </summary>
    public interface IOnNavigatedFrom
    {
        /// <summary>
        /// Called by <see cref="INavigationService"/> when a new <see cref="INavigationModule"/>
        /// is displayed, and the previous module is about to be uninitialized. At this point, the 
        /// navigation process cannot be rejected or redirected.
        /// </summary>
        /// <param name="e">Provides navigation context of method call.</param>
        public void OnNavigatedFrom(OnNavigatedFromEventArgs e);
    }
}
