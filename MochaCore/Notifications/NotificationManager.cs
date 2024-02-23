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
        private static bool _isSetup = false;
        private static Dictionary<string, Func<INotification>> _builders = new();
        private static Dictionary<string, List<INotification>> _notifications = new();

        /// <summary>
        /// Occurs when any notification associated with the current application has been interacted with by the user.
        /// </summary>
        public static event EventHandler<AppNotificationInteractedEventArgs>? NotificationInteracted;

        public static void Setup(INotificationSetupProvider setupProvider)
        {
            if (_isSetup)
            {
                throw new InvalidOperationException("Notification setup has already been performed. " +
                    "Cannot set up notifications more than once.");
            }

            setupProvider.Setup(RawNotificationHandler);
            _isSetup = true;
        }

        public static void RegisterNotification(string id, Func<INotification> factoryDelegate)
            => RegisterNotificationCore(id, factoryDelegate);

        public static void RegisterNotification<T>(string id, Func<INotification<T>> factoryDelegate) where T : new()
            => RegisterNotificationCore(id, factoryDelegate);

        public static INotification RetrieveNotification(string id)
        {
            SetupGuard();

            INotification createdNotification = GetBuilderOrThrow(id).Invoke();
            TrackNotification(id, createdNotification);
            return createdNotification;
        }

        public static INotification<T> RetrieveNotification<T>(string id) where T : new()
        {
            SetupGuard();

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
            SetupGuard();

            if (_notifications.TryGetValue(id, out List<INotification>? notofications))
            {
                return notofications.ToImmutableArray();
            }

            return new List<INotification>();
        }

        private static void RegisterNotificationCore(string id, Func<INotification> factoryDelegate)
        {
            SetupGuard();

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

            void Disposed(object? sender, EventArgs e)
            {
                _notifications[id].Remove(notification);
                notification.Disposed -= Disposed;
            }
        }

        private static void RawNotificationHandler(RawNotificationInteractedArgs args)
        {
            INotification? notification = _notifications.Values
                .SelectMany(l => l).FirstOrDefault(n => n.Id == args.NotificationId);

            NotificationInteracted?.Invoke(null, new AppNotificationInteractedEventArgs(args.NotificationId, notification));
        }

        private static void SetupGuard()
        {
            if (!_isSetup)
            {
                throw new InvalidOperationException("Notification setup has not been performed.");
            }
        }
    }
}
