using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mocha.Navigation;

namespace Mocha.Navigation
{
    /// <summary>
    /// Allows implementing class to expose an asynchronous version of
    /// *OnNavigatedTo* method which is called by <see cref="NavigationService"/>.
    /// </summary>
    public interface IOnNavigatedToAsync
    {
        /// <summary>
        /// Called when navigation process is about to finish. 
        /// </summary>
        /// <param name="navigationData">Details of navigation request.</param>
        Task OnNavigatedToAsync(NavigationData navigationData);
    }
}
