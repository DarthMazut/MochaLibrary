using System;

namespace MochaCore.Notifications
{
    public class RawNotificationInteractedArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawNotificationInteractedArgs"/> class.
        /// </summary>
        /// <param name="notificationId">Identifier of interacted notification.</param>
        public RawNotificationInteractedArgs(string? notificationId)
        {
            NotificationId = notificationId;
        }

        /// <summary>
        /// Identifier of interacted notification.
        /// </summary>
        public string? NotificationId { get; }

        // TODO: add interaction args
    }
}