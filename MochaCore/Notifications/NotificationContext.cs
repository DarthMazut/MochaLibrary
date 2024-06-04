using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Notifications
{
    /// <summary>
    /// Passes messages between implementations of <see cref="INotification"/> and <see cref="NotificationManager"/>
    /// </summary>
    public class NotificationContext
    {
        private readonly Action<NotificationInteractedEventArgs> _interactionHandler;

        public NotificationContext(string registrationId) : this(registrationId, null) { }


        public NotificationContext(string registrationId, Action<NotificationInteractedEventArgs>? interactionHandler)
        {
            RegistrationId = registrationId;
            _interactionHandler = interactionHandler;
        }

        /// <summary>
        /// Identifier used for registration of related <see cref="INotification"/> builder.
        /// </summary>
        public string RegistrationId { get; }

        /// <summary>
        /// Invokes interaction handler provided during construction of this object, if any.
        /// </summary>
        /// <remarks>
        /// This method is to notify <see cref="NotificationManager"/> that any notification
        /// associated with current <see cref="RegistrationId"/> has been activated. <see cref="NotificationManager"/>
        /// creates single instance of <see cref="INotification"/> for each registration ID and provides a callback
        /// during creation. This callback should be invoked for every interacted notification with same registration ID.
        /// This call allows <see cref="NotificationManager.NotificationInteracted"/> event to be appropriately triggered.
        /// </remarks>
        /// <param name="eventArgs">Interaction arguments.</param>
        public void NotifyInteracted(NotificationInteractedEventArgs eventArgs)
        {
            _interactionHandler?.Invoke(eventArgs);
        }
    }
}
