using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using MochaCore.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace MochaWinUI.Notifications.Extensions
{
    /// <summary>
    /// Contains extension methods for working with the <see cref="WinUiNotification"/> class or its descendants.
    /// </summary>
    public static class WinUiNotificationExtensions
    {
        /// <summary>
        /// Adds key-value arguments into creating <see cref="AppNotification"/>. The following keys are added:
        /// <see cref="WinUiNotification.NotificationIdKey"/>, <see cref="WinUiNotification.RegistrationIdKey"/>, 
        /// <see cref="WinUiNotification.InvokedItemIdKey"/> with the values obtained from provided <paramref name="notification"/>.
        /// If <see cref="MochaCore.Notifications.INotification.Tag"/> is specified within provided <paramref name="notification"/>
        /// it is also added as argument into creating instance.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="notification">Notification which provides values for adding arguments.</param>
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
        /// Retrieves a specific value from the content of an <see cref="AppNotification"/> object based on the provided key.
        /// </summary>
        /// <param name="notification">The <see cref="AppNotification"/> object from which to retrieve the value.</param>
        /// <param name="key">The key corresponding to the value being searched within the notification content.</param>
        /// <returns>
        /// The value associated with the provided <paramref name="key"/> within the <paramref name="notification"/> content,
        /// or <see langword="null"/> if the key is not found or the notification content is invalid.
        /// </returns>
        public static string? GetValueByKey(this AppNotification notification, string key)
        {
            XmlDocument xml = new();
            xml.LoadXml(notification.Payload);
            return GetValueByKey(xml, key);
        }

        /// <summary>
        /// Retrieves a specific value from the content of a <see cref="ScheduledToastNotification"/> object based on the provided key.
        /// </summary>
        /// <param name="notification">The <see cref="ScheduledToastNotification"/> object from which to retrieve the value.</param>
        /// <param name="key">The key corresponding to the value being searched within the notification content.</param>
        /// <returns>
        /// The value associated with the provided <paramref name="key"/> within the <paramref name="notification"/> content,
        /// or <see langword="null"/> if the key is not found or the notification content is invalid.
        /// </returns>
        public static string? GetValueByKey(this ScheduledToastNotification notification, string key)
            => GetValueByKey(notification.Content, key);

        /// <summary>
        /// Searches the notification content to find the value associated with the notification by the given key.
        /// Returns the found value or <see langword="null"/> if the key is not found.
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="key">The key used to search for the associated value within the notification content.</param>
        public static string? GetValueByKey(this ToastNotification notification, string key)
            => GetValueByKey(notification.Content, key);

        /// <summary>
        /// Determines whether content of <see cref="ScheduledTileNotification"/> provides
        /// a values for following keys: <see cref="WinUiNotification.NotificationIdKey"/>,
        /// <see cref="WinUiNotification.RegistrationIdKey"/> and <see cref="WinUiNotification.InvokedItemIdKey"/>.
        /// </summary>
        public static bool IsValid(this ScheduledToastNotification notification) => ValidateXmlDocument(notification.Content);

        /// <summary>
        /// Checks whether the current instance provides values for the required keys defined by <see cref="WinUiNotification"/>.
        /// </summary>
        public static bool IsValid(this ToastNotification notification) => ValidateXmlDocument(notification.Content);

        /// <summary>
        /// Determines whether <see cref="AppNotificationActivatedEventArgs"/> object
        /// contains keys required to be processed by <see cref="WinUiNotification"/>
        /// implementations.
        /// </summary>
        public static bool AreValid(this AppNotificationActivatedEventArgs args)
        {
            bool hasNotificationId = args.Arguments.ContainsKey(WinUiNotification.NotificationIdKey);
            bool hasRegistrationId = args.Arguments.ContainsKey(WinUiNotification.RegistrationIdKey);
            bool hasInvokedItemId = args.Arguments.ContainsKey(WinUiNotification.InvokedItemIdKey);

            return hasNotificationId && hasRegistrationId && hasInvokedItemId;
        }

        /// <summary>
        /// Flattens <see cref="AppNotificationActivatedEventArgs"/> into single <c>Dictionary&lt;string, object&gt;</c>.
        /// </summary>
        public static Dictionary<string, object> AsDictionary(this AppNotificationActivatedEventArgs args)
        {
            Dictionary<string, object> dictionary = args.Arguments.ToDictionary(kvp => kvp.Key, kvp => kvp.Value as object);
            foreach ((string key, string value) in args.UserInput)
            {
                if (!dictionary.TryAdd(key, value))
                {
                    dictionary[key] = new string[2] { (string)dictionary[key], value };
                }
            }

            return dictionary;
        }

        private static string? GetValueByKey(XmlDocument xml, string key)
            => xml.FirstChild.Attributes[0].InnerText
                .Split(";")
                .Select(s => s.Split("="))
                .Where(arr => arr[0] == key)
                ?.FirstOrDefault()?[1];

        private static bool ValidateXmlDocument(XmlDocument xml)
        {
            IXmlNode? firstAttr = xml.FirstChild.Attributes.FirstOrDefault();
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
