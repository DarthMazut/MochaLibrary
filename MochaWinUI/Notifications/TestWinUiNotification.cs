using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using MochaCore.Notifications;
using MochaWinUI.Notifications.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace MochaWinUI.Notifications
{
    public class MyNotificationSettings
    {
        public string CustomSetting { get; set; }
    }

    public class MyNotificationCustomEventArgs
    {
        public string EventCustomables { get; set; }
    }

    public class TestWinUiNotification : WinUiNotification<MyNotificationSettings, MyNotificationCustomEventArgs>
    {
        public TestWinUiNotification(string registrationId) : base(registrationId) { }

        public TestWinUiNotification(string notificationId, string registrationId, string? tag, DateTimeOffset scheduledTime, bool wasDisplayed, bool wasInteracted)
            : base(notificationId, registrationId, tag, scheduledTime, wasDisplayed, wasInteracted) { }

        protected override string CreateNotificationDefinition()
        {
            AppNotification appNotification = new AppNotificationBuilder()
            .AddNotificationArguments(this)
            .AddText("Test!!!")
            .AddButton(
                new AppNotificationButton("Test Button")
                .AddElementArguments(this, "Button")
                .SetToolTip("Test tooltip !!!"))
            .SetAppLogoOverride(new Uri(@"C:\Users\Ellie\Desktop\test.png"))
            .BuildNotification();

            return appNotification.Payload;
        }

        protected override NotificationInteractedEventArgs<MyNotificationCustomEventArgs> CreateArgsFromInteractedEvent(AppNotificationActivatedEventArgs args)
        {
            args.Arguments.TryGetValue(TagKey, out string? tag);

            return new NotificationInteractedEventArgs<MyNotificationCustomEventArgs>(
                new TestWinUiNotification(
                    args.Arguments[NotificationIdKey],
                    args.Arguments[RegistrationIdKey],
                    tag, DateTimeOffset.UtcNow, true, true),
                args.Arguments[InvokedItemIdKey],
                CreateArgsDictionary(args),
                args, new MyNotificationCustomEventArgs() { EventCustomables = "Custom ;)"});
        }

        protected override INotification? CreatePendingNotification(ScheduledToastNotification notification)
        {
            if (!notification.IsValid())
            {
                return null;
            }

            return new TestWinUiNotification(
                notification.GetNotificationValueByKey(RegistrationIdKey)!,
                notification.GetNotificationValueByKey(NotificationIdKey)!,
                notification.GetNotificationValueByKey(TagKey),
                notification.DeliveryTime,
                false, false);
        }
    }
}
