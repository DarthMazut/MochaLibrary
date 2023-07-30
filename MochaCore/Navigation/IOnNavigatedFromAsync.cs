using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Allows an implementing class to expose an asynchronous version of the <c>OnNavigatedFrom()</c> 
    /// method, which is called by <see cref="INavigationService"/> whenever the navigation process
    /// is about to complete.
    /// </summary>
    public interface IOnNavigatedFromAsync
    {
        /// <summary>
        /// Called by <see cref="INavigationService"/> when a new <see cref="INavigationModule"/>
        /// is displayed, and the previous module is about to be uninitialized. At this point, the 
        /// navigation process cannot be rejected or redirected.
        /// </summary>
        /// <param name="e">Provides navigation context of method call.</param>
        public Task OnNavigatedFromAsync(OnNavigatedFromEventArgs e);
    }
}
