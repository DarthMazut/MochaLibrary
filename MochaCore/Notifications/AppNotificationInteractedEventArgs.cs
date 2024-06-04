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
        /// <param name="notificationId">Identifier of the interacted notification.</param>
        /// <param name="invokedItemId">
        /// Identifier of the element that was invoked on the notification, triggering its activation.
        /// </param>
        /// <param name="asDictionary">Interaction data represented as a dictionary.</param>
        /// <param name="rawArgs">Original arguments of technology-specific interaction event.</param>
        public AppNotificationInteractedEventArgs(
            string notificationId,
            string invokedItemId,
            IReadOnlyDictionary<string, object> asDictionary,
            object? rawArgs)
        {
            NotificationId = notificationId;
            InvokedItemId = invokedItemId;
            AsDictionary = asDictionary;
            RawArgs = rawArgs;
        }


        /// <summary>
        /// Identifier of the interacted notification.
        /// </summary>
        public string NotificationId { get; }

        /// <summary>
        /// Tag assigned to interacted notification, if any.
        /// </summary>
        public string? Tag { get; init; }

        /// <summary>
        /// Gets the interacted notification object, if available.
        /// </summary>
        public INotification? Notification { get; init; }

        /// <summary>
        /// Identifier of the element that was invoked on the notification, triggering its activation.
        /// </summary>
        public string InvokedItemId { get; }

        /// <summary>
        /// Text entered by the user on the displayed notification 
        /// or <see langword="null"/> if no such was provided.
        /// </summary>
        public string? TextInput { get; init; }

        /// <summary>
        /// Identifier of the item selected during notification interaction, 
        /// often from a ComboBox or similar widgets or <see langword="null"/> if no such was selected.
        /// </summary>
        public string? SelectedItemId { get; init; }

        /// <summary>
        /// The date selected during interaction with a notification, if any.
        /// </summary>
        public DateTimeOffset? SelectedDate { get; init; }

        /// <summary>
        /// Gets the interaction data represented as a dictionary.
        /// </summary>
        public IReadOnlyDictionary<string, object> AsDictionary { get; }

        /// <summary>
        /// Original arguments of technology-specific interaction event.
        /// </summary>
        public object? RawArgs { get; }
    }

    /// <summary>
    /// Provides arguments for the <see cref="NotificationManager.NotificationInteracted"/> event.
    /// </summary>
    /// <typeparam name="T">Type of detailed interaction data.</typeparam>
    public class AppNotificationInteractedEventArgs<T> : AppNotificationInteractedEventArgs
    {
        public AppNotificationInteractedEventArgs(
            string notificationId,
            string invokedItemId,
            IReadOnlyDictionary<string, object> asDictionary,
            object? rawArgs,
            T interactionData)
                : base(notificationId, invokedItemId, asDictionary, rawArgs)
        {
            InteractionData = interactionData;
        }

        /// <summary>
        /// Detailed data regarding user interaction with the notification.
        /// </summary>
        public T InteractionData { get; }
    }
}