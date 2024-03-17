using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using MochaCore.Notifications;
using MochaCore.Notifications.Extensions;
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
        private readonly NotificationContext? _context;

        private string? _tag;
        private DateTimeOffset? _scheduledTime;
        private bool _displayed;
        private bool _isDisposed;

        static WinUiNotification()
        {
            try
            {
                AppNotificationManager.Default.NotificationInvoked += (s, e) => { };
                AppNotificationManager.Default.Register();
            }
            catch (Exception ex) when (ex.InnerException?.HResult == 0) // TODO: number
            {
                // We're already registered so just ignore :)
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNotification{T}"/> class.
        /// </summary>
        public WinUiNotification(NotificationContext relay)
        {
            _context = relay;
            _registrationId = relay.RegistrationdId;
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
        public string? Tag
        {
            get => _tag;
            set
            {
                // In this case we need to reschedule in case this changes after scheduling,
                // it might be better to disallow tag change after Schedule(),
                // but on the other side if we can Schedule() to reschedule, why shouldn't we allow for this?
                if (_displayed)
                {
                    throw new InvalidOperationException("Cannot change tag after notification has been displayed");
                }

                _tag = value;
            }
        }

        /// <inheritdoc/>
        public event EventHandler<NotificationInteractedEventArgs>? Interacted;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <inheritdoc/>
        public void Schedule()
        {
            AppNotification appNotification = new AppNotificationBuilder()
            .AddArgument(NOTIFICATION_ID, Id)
            .AddArgument(REGISTRATION_ID, _registrationId)
            .AddArgument(INVOKED_ITEM_ID, Id)
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
        public void Schedule(DateTimeOffset scheduledTime)
        {
            _scheduledTime = scheduledTime;
            throw new NotImplementedException();
        }

        private void ScheduleCore(DateTimeOffset? scheduledTime)
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
                _context!.NotifyInteracted(CreateEventArgsFromRawEvent(args));
            }

            if (args.Arguments[NOTIFICATION_ID] == Id)
            {
                _displayed = true;
                Interacted?.Invoke(this, CreateEventArgsFromRawEvent(args).WithNotification(this));
            }
        }

        private static NotificationInteractedEventArgs CreateEventArgsFromRawEvent(AppNotificationActivatedEventArgs args)
        {
            return new NotificationInteractedEventArgs(
                CreateNotificationFromRawEvent(args),
                args.Arguments[INVOKED_ITEM_ID],
                CreateArgsDictionary(args),
                args);
        }

        private static Dictionary<string, object> CreateArgsDictionary(AppNotificationActivatedEventArgs args)
        {
            // TODO
            return args.Arguments.ToDictionary(kvp => kvp.Key, kvp => kvp.Value as object);
        }

        private static INotification<T> CreateNotificationFromRawEvent(AppNotificationActivatedEventArgs args)
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
