using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NotificationBuilder = System.Func<string, System.Action<MochaCore.Notifications.AppNotificationInteractedEventArgs>?, MochaCore.Notifications.INotification>;

namespace MochaCore.Notifications
{
    /// <summary>
    /// Register your notification implementations and retrieve it later via abstraction.
    /// Manage your notifications.
    /// </summary>
    public static class NotificationManager
    {
        private static Dictionary<string, NotificationBuilder> _builders = new();
        private static Dictionary<string, List<INotification>> _notifications = new();

        /// <summary>
        /// Occurs when any notification associated with the current application has been interacted with by the user.
        /// </summary>
        public static event EventHandler<AppNotificationInteractedEventArgs>? NotificationInteracted;

        public static void RegisterNotification(string id, NotificationBuilder factoryDelegate)
            => RegisterNotificationCore(id, factoryDelegate);

        public static void RegisterNotification<T>(string id, NotificationBuilder factoryDelegate) where T : new()
            => RegisterNotificationCore(id, factoryDelegate);

        public static INotification RetrieveNotification(string id)
        {
            INotification createdNotification = GetBuilderOrThrow(id).Invoke(id, null);
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

        /// <summary>
        /// Retrieves all instantiated <see cref="INotification"/> objects that hasn't been disposed yet.
        /// </summary>
        public static IReadOnlyCollection<INotification> GetCreatedNotifications()
            => _notifications.Values.SelectMany(n => n).ToImmutableArray();

        /// <summary>
        /// Retrieves all instantiated <see cref="INotification"/> objects registerd 
        /// by given ID that hasn't been disposed yet.
        /// </summary>
        /// <param name="id">Registration ID of requested notifications.</param>
        public static IReadOnlyCollection<INotification> GetCreatedNotifications(string id)
        {
            if (_notifications.TryGetValue(id, out List<INotification>? notofications))
            {
                return notofications.ToImmutableArray();
            }

            return new List<INotification>();
        }

        private static void RegisterNotificationCore(string id, NotificationBuilder factoryDelegate)
        {
            if (_builders.ContainsKey(id))
            {
                throw new InvalidOperationException($"Notification factory delegate with id={id} has been already registered");
            }

            _builders[id] = factoryDelegate;
            _ = factoryDelegate.Invoke(id, (e) => NotificationInteracted?.Invoke(null, e));
        }

        private static NotificationBuilder GetBuilderOrThrow(string id)
        {
            if (_builders.TryGetValue(id, out NotificationBuilder? builder))
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

            void Disposed(object? sender, EventArgs e)
            {
                _notifications[id].Remove(notification);
                notification.Disposed -= Disposed;
            }
        }
    }
}
