using Microsoft.Windows.AppNotifications;
using MochaCore.Notifications;
using MochaWinUI.Notifications.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace MochaWinUI.Notifications
{
    /// <summary>
    /// Encapsulates data required for creating instance of <see cref="MochaCore.Notifications.INotification"/>
    /// which represents existing notification.
    /// </summary>
    public class NotificationCreationData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationCreationData"/> class.
        /// </summary>
        /// <param name="dataSource">The data source used to create the current object.</param>
        public NotificationCreationData(AppNotificationActivatedEventArgs dataSource)
        {
            dataSource.Arguments.TryGetValue(WinUiNotification.TagKey, out string? tag);

            InteractedDataSource = dataSource;
            NotificationId = dataSource.Arguments[WinUiNotification.NotificationIdKey];
            RegistrationId = dataSource.Arguments[WinUiNotification.RegistrationIdKey];
            Tag = tag;
            ScheduledTime = DateTimeOffset.UtcNow;
            WasDisplayed = true;
            WasInteracted = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationCreationData"/> class.
        /// </summary>
        /// <param name="dataSource">The data source used to create the current object.</param>
        public NotificationCreationData(ToastNotification dataSource)
        {
            DisplayedDataSource = dataSource;
            NotificationId = dataSource.GetValueByKey(WinUiNotification.NotificationIdKey)!;
            RegistrationId = dataSource.GetValueByKey(WinUiNotification.RegistrationIdKey)!;
            Tag = dataSource.GetValueByKey(WinUiNotification.TagKey);
            // https://github.com/DarthMazut/MochaLibrary/issues/19
            ScheduledTime = default;
            WasDisplayed = true;
            WasInteracted = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationCreationData"/> class.
        /// </summary>
        /// <param name="dataSource">The data source used to create the current object.</param>
        public NotificationCreationData(ScheduledToastNotification dataSource)
        {
            PendingDataSource = dataSource;
            NotificationId = dataSource.GetValueByKey(WinUiNotification.NotificationIdKey)!;
            RegistrationId = dataSource.GetValueByKey(WinUiNotification.RegistrationIdKey)!;
            Tag = dataSource.GetValueByKey(WinUiNotification.TagKey);
            ScheduledTime = dataSource.DeliveryTime;
            WasDisplayed = false;
            WasInteracted = false;
        }

        /// <summary>
        /// The unique identifier for the notification.
        /// </summary>
        public string NotificationId { get; }

        /// <summary>
        /// The identifier assigned during registration with the <see cref="NotificationManager"/>.
        /// </summary>
        public string RegistrationId { get; }

        /// <summary>
        /// An optional tag associated with the notification.
        /// </summary>
        public string? Tag { get; }

        /// <summary>
        /// The scheduled time for the notification.
        /// </summary>
        public DateTimeOffset ScheduledTime { get; }

        /// <summary>
        /// Whether this notification was already displayed and so cannot be scheduled again.
        /// </summary>
        public bool WasDisplayed { get; }

        /// <summary>
        /// Whether this notification was already interacted by the user.
        /// </summary>
        public bool WasInteracted { get; }

        /// <summary>
        /// Returns the object that is the data source for the current instance, 
        /// or <see langword="null"/> if an object of this type was not such a source.
        /// </summary>
        public AppNotificationActivatedEventArgs? InteractedDataSource { get; }

        /// <summary>
        /// Returns the object that is the data source for the current instance, 
        /// or <see langword="null"/> if an object of this type was not such a source.
        /// </summary>
        public ToastNotification? DisplayedDataSource { get; }

        /// <summary>
        /// Returns the object that is the data source for the current instance, 
        /// or <see langword="null"/> if an object of this type was not such a source.
        /// </summary>
        public ScheduledToastNotification? PendingDataSource { get; }
    }
}
