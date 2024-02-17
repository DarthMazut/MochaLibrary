using System;

namespace MochaCore.Notifications
{
    public class RawNotificationInteractedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawNotificationInteractedEventArgs"/> class.
        /// </summary>
        /// <param name="notificationId">Identifier of interacted notification.</param>
        public RawNotificationInteractedEventArgs(string notificationId)
        {
            NotificationId = notificationId;
        }

        /// <summary>
        /// Identifier of interacted notification.
        /// </summary>
        public string NotificationId { get; }

        // TODO: add interaction args
    }
}