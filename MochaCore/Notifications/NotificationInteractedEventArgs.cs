using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Notifications
{
    /// <summary>
    /// Provides data for the event that occurs when a notification is interacted with.
    /// </summary>
    public class NotificationInteractedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationInteractedEventArgs"/> class.
        /// </summary>
        /// <param name="notification">Interacted notification.</param>
        /// <param name="invokedItemId">Identifier of the element that was invoked on the notification, triggering its activation.</param>
        /// <param name="asDictionary">Interaction data represented as a dictionary.</param>
        /// <param name="rawArgs">Original arguments of technology-specific interaction event.</param>
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
        /// Interacted notification.
        /// </summary>
        public INotification Notification { get; }

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
    /// Provides data for the event that occurs when a notification is interacted with.
    /// </summary>
    /// <typeparam name="T">Type of detailed interaction data.</typeparam>
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
