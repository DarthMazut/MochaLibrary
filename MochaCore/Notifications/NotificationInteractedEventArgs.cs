using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Notifications
{
    public class NotificationInteractedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationInteractedEventArgs"/> class.
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="invokedItemId"></param>
        /// <param name="asDictionary"></param>
        /// <param name="rawArgs"></param>
        public NotificationInteractedEventArgs(
            INotification notification,
            string invokedItemId,
            IReadOnlyDictionary<string, object> asDictionary,
            object? rawArgs)
        {
            Notification = notification;
            InvokedItemId = invokedItemId;
            AsDictionary = asDictionary;
            RawArgs = rawArgs;
        }

        /// <summary>
        /// 
        /// </summary>
        public INotification Notification { get; }

        /// <summary>
        /// 
        /// </summary>
        public string InvokedItemId { get; }

        /// <summary>
        /// Text entered by the user on the displayed notification 
        /// or <see langword="null"/> if no such was provided.
        /// </summary>
        public string? TextInput { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string? SelectedItemId { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset? SlectedDate { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyDictionary<string, object> AsDictionary { get; }

        /// <summary>
        /// Original arguments of technology-specific interaction event.
        /// </summary>
        public object? RawArgs { get; }
    }

    public class NotificationInteractedEventArgs<T> : NotificationInteractedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationInteractedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="invokedItemId"></param>
        /// <param name="asDictionary"></param>
        /// <param name="rawArgs"></param>
        /// <param name="interactionData">Detailed data regarding user interaction with the notification.</param>
        public NotificationInteractedEventArgs(
            INotification notification,
            string invokedItemId,
            IReadOnlyDictionary<string, object> asDictionary,
            object? rawArgs,
            T interactionData)
                : base(notification, invokedItemId, asDictionary, rawArgs)
        {
            InteractionData = interactionData;
        }

        /// <summary>
        /// Detailed data regarding user interaction with the notification.
        /// </summary>
        public T InteractionData { get; }
    }
}
