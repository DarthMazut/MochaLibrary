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
            RegistrationdId = registrationId;
            _interactionHandler = interactionHandler;
        }

        public string RegistrationdId { get; }

        public void NotifyInteracted(NotificationInteractedEventArgs eventArgs)
        {
            _interactionHandler?.Invoke(eventArgs);
        }
    }
}
