using Microsoft.Windows.AppNotifications;
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
    public abstract class WinUiNotification : INotification
    {
        private static readonly int ALREADY_REGISTERED_EXCEPTION_CODE = -2147024809;

        public static readonly string NotificationIdKey = "notification-id";
        public static readonly string RegistrationIdKey = "registration-id";
        public static readonly string InvokedItemIdKey = "invoked-item-id";
        public static readonly string TagKey = "tag";

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
        /// Initializes a new instance of the <see cref="WinUiNotification"/> class.
        /// </summary>
        public WinUiNotification(NotificationContext context)
        {
            _context = context;
            RegistrationId = context.RegistrationId;
            Id = Guid.NewGuid().ToString();
            AppNotificationManager.Default.NotificationInvoked += AnyNotificationInvoked;
        }

        protected WinUiNotification(string notificationId, string registrationId, string? tag, DateTimeOffset scheduledTime)
        {
            Id = notificationId;
            RegistrationId = registrationId;
            Tag = tag;
            _scheduledTime = scheduledTime;
            _displayed = true;
        }

        /// <inheritdoc/>
        public string Id { get; }
        
        public string RegistrationId { get; }

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

        protected abstract string CreateNotification();

        protected abstract NotificationInteractedEventArgs CreateEventArgsFromRawEvent(AppNotificationActivatedEventArgs args);

        protected Dictionary<string, object> CreateArgsDictionary(AppNotificationActivatedEventArgs args)
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

            if (args.Arguments[RegistrationIdKey] == RegistrationId)
            {
                _context!.NotifyInteracted(CreateEventArgsFromRawEvent(args));
            }

            if (args.Arguments[NotificationIdKey] == Id)
            {
                _displayed = true;
                Interacted?.Invoke(this, CreateEventArgsFromRawEvent(args).WithNotification(this));
            }
        }

        private bool ValidateArgs(AppNotificationActivatedEventArgs args)
        {
            bool hasNotificationId = args.Arguments.ContainsKey(NotificationIdKey);
            bool hasRegistrationId = args.Arguments.ContainsKey(RegistrationIdKey);
            bool hasInvokedItemId = args.Arguments.ContainsKey(InvokedItemIdKey);

            return hasNotificationId && hasRegistrationId && hasInvokedItemId;
        }
    }

    public abstract class WinUiNotification<T> : WinUiNotification, INotification<T> where T : new()
    {
        public WinUiNotification(NotificationContext context)
            : base(context) { }

        protected WinUiNotification(string notificationId, string registrationId, string? tag, DateTimeOffset scheduledTime)
            : base(notificationId, registrationId, tag, scheduledTime) { }

        /// <inheritdoc/>
        public T Properties { get; set; } = new();
    }

    public abstract class WinUiNotification<TProps, TArgs> : WinUiNotification<TProps>, INotification<TProps, TArgs> where TProps : new()
    {
        protected WinUiNotification(NotificationContext context)
            : base(context) { }

        protected WinUiNotification(string notificationId, string registrationId, string? tag, DateTimeOffset scheduledTime)
            : base(notificationId, registrationId, tag, scheduledTime) { }

        event EventHandler<NotificationInteractedEventArgs<TArgs?>>? INotification<TProps, TArgs>.Interacted
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }
    }

}
