using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Notifications
{
    /// <summary>
    /// Defines API utilized by <see cref="NotificationManager"/> for global notification handling.
    /// This is required for case when application is being restarted after notification 
    /// schedule but before its interaction.
    /// </summary>
    public interface INotificationSharedDataProvider
    {
        /// <summary>
        /// Occurs when any notification registered with implementations id has been interacted with by the user.
        /// </summary>
        public event EventHandler<NotificationInteractedEventArgs> NotificationInteracted;

        /// <summary>
        /// Returns a collection of scheduled notification, registered with implementations id, but not yet displayed.
        /// </summary>
        public IReadOnlyCollection<INotification> GetPendingNotifications();

        /// <summary>
        /// Returns a read-only collection of <see cref="INotification"/> objects representing notifications 
        /// currently stored within the action center that match the registration ID of the implementing class.
        /// </summary>
        public IReadOnlyCollection<INotification> GetActionCenterNotifications();
    }
}
