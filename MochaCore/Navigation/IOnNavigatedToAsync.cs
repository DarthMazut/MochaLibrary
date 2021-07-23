using System.Threading.Tasks;

namespace MochaCore.Navigation
{
    /// <summary>
    /// Allows implementing class to expose an asynchronous version of
    /// <c>OnNavigatedTo()</c> method which is called by <see cref="NavigationService"/> 
    /// whenever navigation process is about to finish.
    /// </summary>
    public interface IOnNavigatedToAsync
    {
        /// <summary>
        /// Called when navigation process is about to finish and new view is displayed.
        /// This event should contain a set up code for current <see cref="INavigatable"/>
        /// including event subscribtion. At this point navigation process cannot be rejected. 
        /// </summary>
        /// <param name="navigationData">Details of navigation request.</param>
        Task OnNavigatedToAsync(NavigationData navigationData);
    }
}
