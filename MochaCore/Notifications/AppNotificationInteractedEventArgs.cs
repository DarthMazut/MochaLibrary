using System;
using System.Collections.Generic;

namespace MochaCore.Notifications
{
    /// <summary>
    /// Provides arguments for the <see cref="NotificationManager.NotificationInteracted"/> event.
    /// </summary>
    public class AppNotificationInteractedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppNotificationInteractedEventArgs"/> class.
        /// </summary>
        /// <param name="notificationId">The identifier of the interacted notification.</param>
        /// <param name="notification">The interacted notification object.</param>
        public AppNotificationInteractedEventArgs(string? notificationId, INotification? notification)
        {
            NotificationId = notificationId;
            Notification = notification;
        }

        /// <summary>
        /// Gets the identifier of the interacted notification.
        /// </summary>
        public string NotificationId { get; }

        /// <summary>
        /// Tag assigned to interacted notification.
        /// </summary>
        public string Tag { get; }

        /// <summary>
        /// Gets the interacted notification object, if available.
        /// </summary>
        public INotification? Notification { get; }

        public IReadOnlyDictionary<string, object> AsDictionary { get; }

        public object? RawArgs { get; }
    }

    public class AppNotificationInteractedEventArgs<T> : AppNotificationInteractedEventArgs
    {
        public T InteractionData { get; }
    }
}