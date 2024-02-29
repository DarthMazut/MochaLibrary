using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Notifications
{
    /// <summary>
    /// Provides a mechanism for subscribing to generic interaction event of notification.
    /// </summary>
    public interface INotificationTracker
    {
        /// <summary>
        /// Occurs when any notification of a specific type is interacted with by the user.
        /// </summary>
        public event EventHandler<NotificationInteractedEventArgs>? GenericNotificationInteracted;
    }
}
