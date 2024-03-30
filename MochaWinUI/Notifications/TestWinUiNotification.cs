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
        public TestWinUiNotification(NotificationContext context)
            : base(context) { }

        protected TestWinUiNotification(string notificationId, string registrationId, string? tag, DateTimeOffset scheduledTime)
            : base(notificationId, registrationId, tag, scheduledTime) { }

        protected override string CreateNotification()
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

        protected override NotificationInteractedEventArgs<MyNotificationCustomEventArgs> CreateEventArgsFromRawEvent(AppNotificationActivatedEventArgs args)
        {
            args.Arguments.TryGetValue(TagKey, out string? tag);

            return new NotificationInteractedEventArgs<MyNotificationCustomEventArgs>(
                new TestWinUiNotification(
                    args.Arguments[NotificationIdKey],
                    args.Arguments[RegistrationIdKey],
                    tag,
                    DateTimeOffset.UtcNow),
                args.Arguments[InvokedItemIdKey],
                CreateArgsDictionary(args),
                args, new MyNotificationCustomEventArgs() { EventCustomables = "Custom ;)"});
        }
    }
}
