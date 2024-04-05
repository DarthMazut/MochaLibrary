using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using System;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

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

        public static string? GetNotificationValueByKey(this AppNotification notification, string key)
        {
            XmlDocument xml = new();
            xml.LoadXml(notification.Payload);
            return xml.FirstChild.Attributes[0].InnerText
                .Split(";")
                .Select(s => s.Split("="))
                .Where(arr => arr[0] == key)
                ?.FirstOrDefault()?[1];
        }

        public static string? GetNotificationValueByKey(this ScheduledToastNotification notification, string key)
        {
            return notification.Content.FirstChild.Attributes[0].InnerText
                .Split(";")
                .Select(s => s.Split("="))
                .Where(arr => arr[0] == key)
                ?.FirstOrDefault()?[1];
        }

        public static bool IsValid(this ScheduledToastNotification notification)
        {
            return
                notification.Content.FirstChild.Attributes.Count > 0 &&
                notification.Content.FirstChild.Attributes[0].InnerText.Split(";").Count() > 3 &&

        }
    }
}
