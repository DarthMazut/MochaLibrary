using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    /// <summary>
    /// Allows an implementing class to expose an asynchronous version of the <c>OnNavigatingFrom()</c> 
    /// method, which is called by <see cref="INavigationService"/> whenever 
    /// navigation to another <see cref="INavigationModule"/> is requested.
    /// </summary>
    public interface IOnNavigatingFromAsync
    {
        /// <summary>
        /// Called whenever the related <see cref="INavigationModule"/> is currently active, and the request has been made 
        /// to navigate to another module. The currently active <see cref="INavigationParticipant"/> can reject or redirect 
        /// the navigation at this point.
        /// </summary>
        public Task OnNavigatingFromAsync(OnNavigatingFromEventArgs e);
    }
}
