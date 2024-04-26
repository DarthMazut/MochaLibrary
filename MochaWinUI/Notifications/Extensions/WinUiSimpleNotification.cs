using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using MochaCore.Notifications;
using MochaCore.Notifications.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace MochaWinUI.Notifications.Extensions
{
    public class WinUiSimpleNotification : WinUiNotification<SimpleNotificationProperties>
    {
        public WinUiSimpleNotification(string registrationId) : base(registrationId) { }

        public WinUiSimpleNotification(
            string notificationId,
            string registrationId,
            string? tag,
            DateTimeOffset scheduledTime,
            bool wasDisplayed,
            bool wasInteracted) 
            : base(notificationId, registrationId, tag, scheduledTime, wasDisplayed, wasInteracted) { }

        /// <inheritdoc/>
        protected override string CreateNotificationDefinition()
        {
            AppNotificationBuilder builder = new();
            builder.AddNotificationArguments(this);
            if (Properties.Title is string title)
            {
                builder.AddText(title);
            }
            
            if (Properties.Content is string content)
            {
                builder.AddText(content);
            }

            if (Properties.Image is string imageUri)
            {
                builder.SetAppLogoOverride(new Uri(imageUri));
            }

            return builder.BuildNotification().Payload;
        }

        /// <inheritdoc/>
        protected override INotification CreateInteractedNotification(AppNotificationActivatedEventArgs args)
        {
            args.Arguments.TryGetValue(TagKey, out string? tag);

            return new WinUiSimpleNotification(
                    args.Arguments[NotificationIdKey],
                    args.Arguments[RegistrationIdKey],
                    tag, DateTimeOffset.UtcNow, true, true);
        }

        /// <inheritdoc/>
        protected override INotification? CreatePendingNotification(ScheduledToastNotification notification)
            => new WinUiSimpleNotification(
                notification.GetValueByKey(RegistrationIdKey)!,
                notification.GetValueByKey(NotificationIdKey)!,
                notification.GetValueByKey(TagKey),
                notification.DeliveryTime,
                false, false);

        /// <inheritdoc/>
        protected override INotification CreateDisplayedNotification(ToastNotification notification)
            => new WinUiSimpleNotification(
                notification.GetValueByKey(RegistrationIdKey)!,
                notification.GetValueByKey(NotificationIdKey)!,
                notification.GetValueByKey(TagKey),
                default, true, false);
    }
}
