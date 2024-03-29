using Microsoft.Windows.AppNotifications.Builder;

namespace MochaWinUI.Notifications.Extensions
{
    public static class WinUiNotificationExtensions
    {
        public static AppNotificationBuilder AddNotificationArguments(this AppNotificationBuilder builder, WinUiNotification notification)
        {
            builder.AddArgument(WinUiNotification.NotificationIdKey, notification.Id)
                   .AddArgument(WinUiNotification.RegistrationIdKey, notification.RegistrationId)
                   .AddArgument(WinUiNotification.InvokedItemIdKey, notification.Id);

            if (notification.Tag is not null)
            {
                builder.AddArgument(WinUiNotification.TagKey, notification.Tag);
            }

            return builder;
        }

        public static AppNotificationButton AddElementArguments(this AppNotificationButton builder, WinUiNotification notification, string elementId)
        {
            builder.AddArgument(WinUiNotification.NotificationIdKey, notification.Id)
                   .AddArgument(WinUiNotification.RegistrationIdKey, notification.RegistrationId)
                   .AddArgument(WinUiNotification.InvokedItemIdKey, elementId);

            if (notification.Tag is not null)
            {
                builder.AddArgument(WinUiNotification.TagKey, notification.Tag);
            }

            return builder;
        }
    }
}
