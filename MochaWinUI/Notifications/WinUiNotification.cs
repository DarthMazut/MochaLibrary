using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using MochaCore.Notifications;
using MochaCore.Notifications.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace MochaWinUI.Notifications
{
    public class WinUiNotification<T> : INotification<T> where T : new()
    {
        private static readonly int ALREADY_REGISTERED_EXCEPTION_CODE = -2147024809;
        private static readonly string NOTIFICATION_ID = "notification-id";
        private static readonly string REGISTRATION_ID = "registration-id";
        private static readonly string INVOKED_ITEM_ID = "invoked-item-id";
        private static readonly string TAG = "tag";

        private readonly string _registrationId;
        private readonly NotificationContext? _context;

        private ScheduledToastNotification? _scheduledNotification;

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
            catch (Exception ex) when (ex.HResult == ALREADY_REGISTERED_EXCEPTION_CODE)
            {
                // We're already registered so just ignore :)
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNotification{T}"/> class.
        /// </summary>
        public WinUiNotification(NotificationContext context)
        {
            _context = context;
            _registrationId = context.RegistrationId;
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
                if (_displayed)
                {
                    throw new InvalidOperationException("Cannot change tag after notification has been displayed");
                }

                _tag = value;

                if (ScheduledTime is not null)
                {
                    Unschedule();
                    Schedule(ScheduledTime.Value);
                } 
            }
        }

        /// <inheritdoc/>
        public event EventHandler<NotificationInteractedEventArgs>? Interacted;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <inheritdoc/>
        public void Schedule()
        {
            ScheduleGuard();

            AppNotification notification = new(CreateNotification());
            _scheduledTime = DateTimeOffset.Now;
            AppNotificationManager.Default.Show(notification);
        }

        /// <inheritdoc/>
        public void Schedule(DateTimeOffset scheduledTime)
        {
            ScheduleGuard();
            Unschedule();

            XmlDocument xml = new();
            xml.LoadXml(CreateNotification());

            ScheduledToastNotification scheduledNotification = new(xml, scheduledTime);
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();

            _scheduledTime = scheduledTime;
            _scheduledNotification = scheduledNotification;
            notifier.AddToSchedule(scheduledNotification);  
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            AppNotificationManager.Default.NotificationInvoked -= AnyNotificationInvoked;
            Unschedule();
            _isDisposed = true;
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual string CreateNotification()
        {
            AppNotification appNotification = new AppNotificationBuilder()
            .AddArgument(NOTIFICATION_ID, Id)
            .AddArgument(REGISTRATION_ID, _registrationId)
            .AddArgument(INVOKED_ITEM_ID, Id)
            .SetTag("DupaTag")
            .AddText("Test!!!")
            .SetHeroImage(new Uri(@"C:\Users\AsyncMilk\Desktop\img_temp.PNG"))
            .AddButton(
                new AppNotificationButton("Test Button")
                .AddArgument("btn", "clicked")
                .SetToolTip("Test tooltip !!!"))
            .BuildNotification();

            return appNotification.Payload;
        }

        private void Unschedule()
        {
            if (_scheduledNotification is not null)
            {
                ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(_scheduledNotification);
            }
        }

        private void ScheduleGuard()
        {
            if (Displayed)
            {
                throw new InvalidOperationException("Displayed notification cannot be rescheduled.");
            }
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
    }

}
