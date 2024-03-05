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
    public class NotificationRelay
    {
        private readonly Action<NotificationInteractedEventArgs> _interactionHandler;

        public NotificationRelay(string registrationId) : this(registrationId, null) { }


        public NotificationRelay(string registrationId, Action<NotificationInteractedEventArgs>? interactionHandler)
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
