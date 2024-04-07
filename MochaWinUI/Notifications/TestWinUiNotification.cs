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

        protected override INotification CreateInteractedNotification(AppNotificationActivatedEventArgs args)
        {
            args.Arguments.TryGetValue(TagKey, out string? tag);

            return new TestWinUiNotification(
                    args.Arguments[NotificationIdKey],
                    args.Arguments[RegistrationIdKey],
                    tag, DateTimeOffset.UtcNow, true, true);
        }

        protected override INotification? CreatePendingNotification(ScheduledToastNotification notification)
            => new TestWinUiNotification(
                notification.GetValueByKey(RegistrationIdKey)!,
                notification.GetValueByKey(NotificationIdKey)!,
                notification.GetValueByKey(TagKey),
                notification.DeliveryTime,
                false, false);

        protected override INotification CreateDisplayedNotification(ToastNotification notification)
        {
            return new TestWinUiNotification(
                notification.GetValueByKey(RegistrationIdKey)!,
                notification.GetValueByKey(NotificationIdKey)!,
                notification.GetValueByKey(TagKey),
                default, true, false);
        }

        protected override MyNotificationCustomEventArgs CreateInteractionData(AppNotificationActivatedEventArgs args)
            => new() { EventCustomables = "Custom ;)" };
    }
}
