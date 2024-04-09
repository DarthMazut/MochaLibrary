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
    /// Allows for managing and registering notification implementations to be retrieved later via abstraction.
    /// </summary>
    public static class NotificationManager
    {
        private static Dictionary<string, Func<INotificationRoot>> _builders = new();
        private static Dictionary<string, List<INotification>> _notifications = new();
        private static Dictionary<string, INotificationSharedDataProvider> _dataProviders = new();

        /// <summary>
        /// Occurs when any notification associated with the current application has been interacted with by the user.
        /// </summary>
        public static event EventHandler<NotificationInteractedEventArgs>? NotificationInteracted;

        /// <summary>
        /// Registers factory delegate which allows for creating new instances of <see cref="INotificationRoot"/>
        /// objects by technology-agnostic side. Use <see cref="RetrieveNotification(string)"/> to obtain new instances,
        /// created by registered delegate, via abstract <see cref="INotification"/> type.
        /// </summary>
        /// <param name="id">Registration identifier.</param>
        /// <param name="factoryDelegate">A delegate to create new instance of <see cref="INotificationRoot"/> implementation.</param>
        public static void RegisterNotification(string id, Func<INotificationRoot> factoryDelegate)
        {
            _ = factoryDelegate ?? throw new ArgumentNullException(nameof(factoryDelegate));

            if (_builders.ContainsKey(id))
            {
                throw new InvalidOperationException($"Notification factory delegate with id={id} has been already registered");
            }

            _builders[id] = factoryDelegate;

            INotificationSharedDataProvider sharedDataNotification = factoryDelegate.Invoke();
            _dataProviders[id] = sharedDataNotification;
            sharedDataNotification.NotificationInteracted += (s, e) =>
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
        /// Returns new instance of <see cref="INotification"/> implementation,
        /// created by factory delegate registered by <see cref="RegisterNotification(string, Func{INotificationRoot})"/> 
        /// with given id.
        /// </summary>
        /// <param name="id">Factory delegate identifier.</param>
        public static INotification RetrieveNotification(string id)
        {
            INotification createdNotification = GetBuilderOrThrow(id).Invoke();
            TrackNotification(id, createdNotification);
            return createdNotification;
        }

        /// <summary>
        /// Returns new instance of <see cref="INotification{T}"/> implementation,
        /// created by factory delegate registered by <see cref="RegisterNotification(string, Func{INotificationRoot})"/> 
        /// with given id.
        /// </summary>
        /// <typeparam name="T">Type of notification properties.</typeparam>
        /// <param name="id">Factory delegate identifier.</param>
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

        public static INotification<TProperties, TArgs> RetrieveNotification<TProperties, TArgs>(string id) where TProperties : new()
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// Returns scheduled notifications, that haven't been displayed yet.
        /// </summary>
        public static Task<IReadOnlyCollection<INotification>> GetPendingNotifications()
        {
            List<INotification> notifications = new();
            foreach ((string id, INotificationSharedDataProvider dataProvider) in _dataProviders)
            {
                notifications.AddRange(dataProvider.GetPendingNotifications()
                    .Select(pn => _notifications[id].FirstOrDefault(n => n.Id == pn.Id) ?? pn)
                    .ToList());
            }

            return Task.FromResult(notifications.AsReadOnly() as IReadOnlyCollection<INotification>);
        }

        /// <summary>
        /// Returns scheduled notifications, registered by provided <paramref name="id"/>, that haven't
        /// been displayed yet.
        /// </summary>
        /// <param name="id">Registration identifier of the notifications to be retrieved.</param>
        public static Task<IReadOnlyCollection<INotification>> GetPendingNotifications(string id)
            => Task.FromResult(_dataProviders[id].GetPendingNotifications()
                .Select(pn => _notifications[id].FirstOrDefault(n => n.Id == pn.Id) ?? pn)
                    .ToList().AsReadOnly() as IReadOnlyCollection<INotification>);

        /// <summary>
        /// Returns notifications that are currently in the Action Center area.
        /// </summary>
        public static Task<IReadOnlyCollection<INotification>> GetDisplayedNotifications()
        {
            List<INotification> notifications = new();
            foreach ((string id, INotificationSharedDataProvider dataProvider) in _dataProviders)
            {
                notifications.AddRange(dataProvider.GetActionCenterNotifications()
                    .Select(pn => _notifications[id].FirstOrDefault(n => n.Id == pn.Id) ?? pn)
                    .ToList());
            }

            return Task.FromResult(notifications.AsReadOnly() as IReadOnlyCollection<INotification>);
        }

        /// <summary>
        /// Returns notifications, registered by provided <paramref name="id"/>, 
        /// that are currently in the Action Center area.
        /// </summary>
        /// <param name="id">Registration identifier of the notifications to be retrieved.</param>
        public static Task<IReadOnlyCollection<INotification>> GetDisplayedNotifications(string id)
            => Task.FromResult(_dataProviders[id].GetActionCenterNotifications()
                .Select(pn => _notifications[id].FirstOrDefault(n => n.Id == pn.Id) ?? pn)
                .ToList().AsReadOnly() as IReadOnlyCollection<INotification>);

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
