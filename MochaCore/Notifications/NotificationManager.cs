using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Notifications
{
    /// <summary>
    /// Register your notification implementations and retrieve it later via abstraction.
    /// Manage your notifications.
    /// </summary>
    public static class NotificationManager
    {
        private static Dictionary<string, Func<INotification>> _builders = new();
        private static Dictionary<string, List<INotification>> _notifications = new();

        public static event EventHandler? NotificationInteracted;

        public static void RegisterNotification(string id, Func<INotification> factoryDelegate)
            => RegisterNotificationCore(id, factoryDelegate);

        public static void RegisterNotification<T>(string id, Func<INotification<T>> factoryDelegate) where T : new()
            => RegisterNotificationCore(id, factoryDelegate);

        public static INotification RetrieveNotification(string id)
        {
            INotification createdNotification = GetBuilderOrThrow(id).Invoke();
            TrackNotification(id, createdNotification);
            return createdNotification;
        }

        public static INotification<T> RetrieveNotification<T>(string id) where T : new()
        {
            if (GetBuilderOrThrow(id) is Func<INotification<T>> typedBuilder)
            {
                INotification<T> notification = typedBuilder.Invoke();
                TrackNotification(id, notification);
                return notification;
            }

            throw new InvalidCastException($"Notification with id={id} was found, but generic parameter was invalid.");
        }

        public static List<INotification> GetCreatedNotifications(string id)
        {
            if (_notifications.TryGetValue(id, out List<INotification>? notofications))
            {
                return notofications;
            }

            return new List<INotification>();
        }

        private static void RegisterNotificationCore(string id, Func<INotification> factoryDelegate)
        {
            if (!_builders.TryAdd(id, factoryDelegate))
            {
                throw new InvalidOperationException($"Notification factory delegate with id={id} has been already registered");
            }
        }

        private static Func<INotification> GetBuilderOrThrow(string id)
        {
            if (_builders.TryGetValue(id, out Func<INotification>? builder))
            {
                return builder;
                
            }

            throw new InvalidOperationException($"Notification with id={id} was never registered.");
        }

        private static void TrackNotification(string id, INotification notification)
        {
            if (!_notifications.TryAdd(id, new List<INotification>() { notification }))
            {
                _notifications[id].Add(notification);
            }

            notification.Disposed += Disposed;
            notification.Interacted += NotificationInteractedCore;

            void Disposed(object? sender, EventArgs e)
            {
                _notifications[id].Remove(notification);
                notification.Disposed -= Disposed;
                notification.Interacted -= NotificationInteractedCore;
            }
        }

        private static void NotificationInteractedCore(object? sender, EventArgs e)
        {
            NotificationInteracted?.Invoke(sender, e);
        }
    }
}
