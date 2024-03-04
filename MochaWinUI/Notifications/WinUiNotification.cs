using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using MochaCore.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.Notifications
{
    public class WinUiNotification<T> : INotification<T> where T : new()
    {
        private static readonly string NOTIFICATION_ID = "notification-id";
        private static readonly string REGISTRATION_ID = "registration-id";
        private static readonly string INVOKED_ITEM_ID = "invoked-item-id";
        private static readonly string TAG = "tag";

        private readonly string _registrationId;
        private readonly Action<NotificationInteractedEventArgs>? _generalHandler;

        private DateTimeOffset? _scheduledTime;
        private bool _displayed;
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNotification{T}"/> class.
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="generalHandler"></param>
        public WinUiNotification(string registrationId, Action<NotificationInteractedEventArgs>? generalHandler)
        {
            _registrationId = registrationId;
            _generalHandler = generalHandler;
            Id = Guid.NewGuid().ToString();
            AppNotificationManager.Default.NotificationInvoked += AnyNotificationInvoked;
        }

        private WinUiNotification(string notificationId, string registrationId, string? tag, DateTimeOffset scheduledTime)
        {
            Id = notificationId;
            _registrationId = registrationId;
            Tag = tag;
            _scheduledTime = scheduledTime;
            _displayed = true;
        }

        /// <inheritdoc/>
        public string Id { get; }

        /// <inheritdoc/>
        public T Properties { get; set; } = new();
        
        /// <inheritdoc/>
        public DateTimeOffset? ScheduledTime => _scheduledTime;

        /// <inheritdoc/>
        public bool Displayed => _displayed;

        /// <inheritdoc/>
        public bool IsDisposed => _isDisposed;

        /// <inheritdoc/>
        public string? Tag { get; }

        /// <inheritdoc/>
        public event EventHandler<NotificationInteractedEventArgs>? Interacted;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <inheritdoc/>
        public void Schedule()
        {
            AppNotification appNotification = new AppNotificationBuilder()
            .AddArgument("id", Id)
            .AddText("Test!!!")
            .SetHeroImage(new Uri(@"C:\Users\AsyncMilk\Desktop\img_temp.PNG"))
            .AddButton(
                new AppNotificationButton("Test Button")
                .AddArgument("btn", "clicked")
                .SetToolTip("Test tooltip !!!"))
            .BuildNotification();

            _scheduledTime = DateTimeOffset.Now;
            AppNotificationManager.Default.Show(appNotification);
        }

        /// <inheritdoc/>
        public void Schedule(string tag)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Schedule(DateTimeOffset scheduledTime)
        {
            _scheduledTime = scheduledTime;
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Schedule(DateTimeOffset scheduledTime, string tag)
        {
            throw new NotImplementedException();
        }

        private void ScheduleCore(DateTimeOffset? scheduledTime, string? tag)
        {
            if (Displayed)
            {
                throw new InvalidOperationException("Displayed notification cannot be rescheduled.");
            }


        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            AppNotificationManager.Default.NotificationInvoked -= AnyNotificationInvoked;
            // If scheduled but not yet displayed unschedule here...
            _isDisposed = true;
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        private void AnyNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
        {
            if (!ValidateArgs(args))
            {
                // We cannot throw here as application may recieve other notification that are valid but not following our design.
                return;
            }

            if (args.Arguments[REGISTRATION_ID] == _registrationId)
            {
                _generalHandler?.Invoke(CreateEventArgsFromRawEvent(args));
            }

            if (args.Arguments[NOTIFICATION_ID] == Id)
            {
                _displayed = true;
                Interacted?.Invoke(this, new NotificationInteractedEventArgs(this, "", new Dictionary<string, object>(), args));
            }
        }

        private static NotificationInteractedEventArgs CreateEventArgsFromRawEvent(AppNotificationActivatedEventArgs args)
        {
            return new NotificationInteractedEventArgs(
                CreateNotificationForRawEvent(args),
                args.Arguments[INVOKED_ITEM_ID],
                CreateArgsDictionary(args),
                args);
        }

        private static Dictionary<string, object> CreateArgsDictionary(AppNotificationActivatedEventArgs args)
        {
            // TODO
            return args.Arguments.ToDictionary(kvp => kvp.Key, kvp => kvp.Value as object);
        }

        private static INotification<T> CreateNotificationForRawEvent(AppNotificationActivatedEventArgs args)
        {
            return new WinUiNotification<T>(
                args.Arguments[NOTIFICATION_ID],
                args.Arguments[REGISTRATION_ID],
                args.Arguments.ToImmutableDictionary().GetValueOrDefault(TAG),
                DateTimeOffset.UtcNow);
        }

        private static bool ValidateArgs(AppNotificationActivatedEventArgs args)
        {
            bool hasNotificationId = args.Arguments.ContainsKey(NOTIFICATION_ID);
            bool hasRegistrationId = args.Arguments.ContainsKey(REGISTRATION_ID);
            bool hasInvokedItemId = args.Arguments.ContainsKey(INVOKED_ITEM_ID);

            return hasNotificationId && hasRegistrationId && hasInvokedItemId;
        }

        private static T CreateInteractionData(AppNotificationActivatedEventArgs args)
        {
            return new T();
        }
    }

}
