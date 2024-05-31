using Microsoft.Windows.AppNotifications;
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

        public NotificationCreationData(ToastNotification dataSource)
        {
            DisplayedDataSource = dataSource;
            NotificationId = dataSource.GetValueByKey(WinUiNotification.NotificationIdKey)!;
            RegistrationId = dataSource.GetValueByKey(WinUiNotification.RegistrationIdKey)!;
            Tag = dataSource.GetValueByKey(WinUiNotification.TagKey);
            // TODO: link ticket here
            ScheduledTime = default;
            WasDisplayed = true;
            WasInteracted = false;
        }

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

        /*
         *         /// <param name="notificationId">The unique identifier for the notification.</param>
        /// <param name="registrationId">The identifier assigned during registration with the <see cref="NotificationManager"/>.</param>
        /// <param name="tag">An optional tag associated with the notification.</param>
        /// <param name="scheduledTime">The scheduled time for the notification.</param>
        /// <param name="wasDisplayed">Whether this notification was already displayed and so cannot be scheduled again.</param>
        /// <param name="wasInteracted">Whether this notification was already interacted by the user.</param>
         */

        public string NotificationId { get; }

        public string RegistrationId { get; }

        public string? Tag { get; }

        public DateTimeOffset ScheduledTime { get; }

        public bool WasDisplayed { get; }

        public bool WasInteracted { get; }

        public AppNotificationActivatedEventArgs? InteractedDataSource { get; }

        public ToastNotification? DisplayedDataSource { get; }

        public ScheduledToastNotification? PendingDataSource { get; }
    }
}
