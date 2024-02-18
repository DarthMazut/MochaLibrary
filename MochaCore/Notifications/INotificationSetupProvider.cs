using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Notifications
{
    /// <summary>
    /// Provides an abstraction for technology-specific implementations of notification setup.
    /// </summary>
    public interface INotificationSetupProvider
    {
        /// <summary>
        /// Sets up the notification handling mechanism.
        /// </summary>
        /// <param name="rawNotificationHandler">
        /// A delegate that is called every time a notification related to the current application
        /// is interacted with by the user.
        /// </param>
        public void Setup(Action<RawNotificationInteractedArgs> rawNotificationHandler);
    }
}
