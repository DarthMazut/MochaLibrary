using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Tries to retrieve value from <see cref="AppNotification"/> content that was specified
        /// by given key. 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="key">Key of the value being searched.</param>
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

        /// <summary>
        /// Determines whether content of <see cref="ScheduledTileNotification"/> provides
        /// a values for following keys: <see cref="WinUiNotification.NotificationIdKey"/>,
        /// <see cref="WinUiNotification.RegistrationIdKey"/> and <see cref="WinUiNotification.InvokedItemIdKey"/>.
        /// </summary>
        public static bool IsValid(this ScheduledToastNotification notification)
        {
            IXmlNode? firstAttr = notification.Content.FirstChild.Attributes.FirstOrDefault();
            if (firstAttr is null) { return false; }

            string[] valuePairs = firstAttr.InnerText.Split(";");
            if (valuePairs.Count() < 3) { return false; }

            List<string> keys = new();
            foreach (string valuePair in valuePairs)
            {
                string[] kvp = valuePair.Split("=");
                if (kvp.Length != 2) { return false; }

                keys.Add(kvp[0]);
            }

            return keys.Contains(WinUiNotification.NotificationIdKey) &&
                   keys.Contains(WinUiNotification.RegistrationIdKey) &&
                   keys.Contains(WinUiNotification.InvokedItemIdKey);
        }
    }
}
