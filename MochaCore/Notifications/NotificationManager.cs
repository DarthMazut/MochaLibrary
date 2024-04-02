using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private static Dictionary<string, Func<INotificationRoot>> _builders = new();
        private static Dictionary<string, List<INotification>> _notifications = new();

        /// <summary>
        /// Occurs when any notification associated with the current application has been interacted with by the user.
        /// </summary>
        public static event EventHandler<NotificationInteractedEventArgs>? NotificationInteracted;

        public static void RegisterNotification(string id, Func<INotificationRoot> factoryDelegate)
        {
            _ = factoryDelegate ?? throw new ArgumentNullException(nameof(factoryDelegate));

            if (_builders.ContainsKey(id))
            {
                throw new InvalidOperationException($"Notification factory delegate with id={id} has been already registered");
            }

            _builders[id] = factoryDelegate;
            factoryDelegate.Invoke().NotificationInteracted += (s, e) =>
            {
                NotificationInteractedEventArgs args = e;
                INotification? existingInstance = _notifications.GetValueOrDefault(id)?.FirstOrDefault(n => n.Id == e.Notification.Id);
                if (existingInstance is not null)
                {
                    args = e.WithNotification(existingInstance);
                }

                NotificationInteracted?.Invoke(null, args);
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static INotification RetrieveNotification(string id)
        {
            INotification createdNotification = GetBuilderOrThrow(id).Invoke();
            TrackNotification(id, createdNotification);
            return createdNotification;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        public static INotification<T> RetrieveNotification<T>(string id) where T : new()
        {
            if (GetBuilderOrThrow(id) is Func<NotificationContext, INotification<T>> typedBuilder)
            {
                INotification<T> notification = typedBuilder.Invoke(new NotificationContext(id));
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

        private static Func<INotificationRoot> GetBuilderOrThrow(string id)
        {
            if (_builders.TryGetValue(id, out Func<INotificationRoot>? builder))
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
