using Microsoft.Windows.AppNotifications;
using MochaCore.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace MochaWinUI.Notifications
{
    /// <summary>
    /// Provides a base implementation of <see cref="INotificationRoot"/> for WinUI.
    /// </summary>
    public abstract class WinUiNotification : INotificationRoot
    {
        private static readonly int ALREADY_REGISTERED_EXCEPTION_CODE = -2147024809;

        /// <summary>
        /// Provides internal key for notification id value.
        /// </summary>
        public static readonly string NotificationIdKey = "notification-id";

        /// <summary>
        /// Provides internal key for registration id value.
        /// </summary>
        public static readonly string RegistrationIdKey = "registration-id";

        /// <summary>
        /// Provides internal key for invoked item id value.
        /// </summary>
        public static readonly string InvokedItemIdKey = "invoked-item-id";

        /// <summary>
        /// Provides internal key for tag value.
        /// </summary>
        public static readonly string TagKey = "tag";

        private ScheduledToastNotification? _scheduledNotification;

        private string? _tag;
        private DateTimeOffset? _scheduledTime;
        private bool _interacted;
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
                // We're already registered.
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNotification"/> class.
        /// </summary>
        /// <param name="registrationId">The identifier assigned during registration with the <see cref="NotificationManager"/>.</param>
        public WinUiNotification(string registrationId)
        {
            Id = Guid.NewGuid().ToString();
            RegistrationId = registrationId;
            AppNotificationManager.Default.NotificationInvoked += AnyNotificationInvoked;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNotification"/> class.
        /// 
        /// <para>
        /// When a client subscribes to the <see cref="NotificationManager.NotificationInteracted"/> event, they expect 
        /// the provided <see cref="NotificationInteractedEventArgs"/> to expose the <see cref="NotificationInteractedEventArgs.Notification"/>
        /// value. Now, when a notification is scheduled and interacted with during the application lifetime, the instance 
        /// created by current constructor is replaced with the tracked instance, ensuring that the client receives the 
        /// actual instance used for scheduling. However, if the application is restarted after the notification is scheduled
        /// but before user interaction, this constructor provides a substitute instance of the notification with limited functionality,
        /// such as the inability to be rescheduled or the <see cref="INotification.Displayed"/> property always being true.
        /// For such scenarios you can leverage current constructor.
        /// </para>
        /// </summary>
        /// <param name="notificationId">The unique identifier for the notification.</param>
        /// <param name="registrationId">The identifier assigned during registration with the <see cref="NotificationManager"/>.</param>
        /// <param name="tag">An optional tag associated with the notification.</param>
        /// <param name="scheduledTime">The scheduled time for the notification.</param>
        protected WinUiNotification(string notificationId, string registrationId, string? tag, DateTimeOffset scheduledTime)
        {
            Id = notificationId;
            RegistrationId = registrationId;
            Tag = tag;
            _scheduledTime = scheduledTime;
            _interacted = true;
        }

        /// <inheritdoc/>
        public string Id { get; }

        /// <summary>
        /// Unique identifier provided during registration with <see cref="NotificationManager"/>.
        /// </summary>
        public string RegistrationId { get; }

        /// <inheritdoc/>
        public DateTimeOffset? ScheduledTime => _scheduledTime;

        // TODO: add Interacted, because why not?

        /// <inheritdoc/>
        public bool Displayed => _interacted || ScheduledTime.HasValue && ScheduledTime.Value < DateTimeOffset.Now;

        /// <inheritdoc/>
        public bool IsDisposed => _isDisposed;

        /// <inheritdoc/>
        public string? Tag
        {
            get => _tag;
            set
            {
                if (Displayed)
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

        // TODO: This should be implemented explicitly
        /// <inheritdoc/>
        public event EventHandler<NotificationInteractedEventArgs>? NotificationInteracted;

        /// <inheritdoc/>
        public void Schedule()
        {
            ScheduleGuard();

            AppNotification notification = new(CreateNotificationDefinition());
            _scheduledTime = DateTimeOffset.Now;
            AppNotificationManager.Default.Show(notification);
        }

        /// <inheritdoc/>
        public void Schedule(DateTimeOffset scheduledTime)
        {
            ScheduleGuard();
            Unschedule();

            XmlDocument xml = new();
            xml.LoadXml(CreateNotificationDefinition());

            ScheduledToastNotification scheduledNotification = new(xml, scheduledTime);
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();

            _scheduledTime = scheduledTime;
            _scheduledNotification = scheduledNotification;
            notifier.AddToSchedule(scheduledNotification);

            ScheduleRemovalFromMessageCenter();
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

        /// <summary>
        /// Override this method to provide a string defining the current appearance of the notification.
        /// </summary>
        protected abstract string CreateNotificationDefinition();

        /// <summary>
        /// Override this method to create a <see cref="NotificationInteractedEventArgs"/> object based on 
        /// the provided <see cref="AppNotificationActivatedEventArgs"/> from the core interaction event.
        /// </summary>
        /// <param name="args">Raw arguments from the core interaction event.</param>
        protected abstract NotificationInteractedEventArgs CreateArgsFromInteractedEvent(AppNotificationActivatedEventArgs args);

        /// <summary>
        /// Flattens the provided <see cref="AppNotificationActivatedEventArgs"/> into dictionary.
        /// </summary>
        /// <param name="args">The arguments from the core interaction event.</param>
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

        /// <summary>
        /// Raises the <see cref="Interacted"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnInteracted(NotificationInteractedEventArgs e)
        {
            Interacted?.Invoke(this, e);
        }

        private void Unschedule()
        {
            if (_scheduledNotification is not null)
            {
                ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(_scheduledNotification);
            }
        }

        private void ScheduleRemovalFromMessageCenter()
        {
            _ = Task.Run(async () =>
            {
                IList<AppNotification> notifications = await AppNotificationManager.Default.GetAllAsync();
                AppNotification? currentNotification = GetCurrentNotificationFromCollection(notifications);
                await AppNotificationManager.Default.RemoveByIdAsync(currentNotification?.Id ?? default);
            });
        }

        private AppNotification? GetCurrentNotificationFromCollection(IEnumerable<AppNotification> collection)
            => collection.FirstOrDefault(n =>
            {
                XmlDocument xml = new();
                xml.LoadXml(n.Payload);
                return xml.FirstChild.Attributes[0].InnerText.Split(";")[1].Split("=")[1] == Id;
            });

        private void ScheduleGuard()
        {
            if (Displayed || IsDisposed)
            {
                throw new InvalidOperationException("Disposed or displayed notification cannot be rescheduled.");
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
                NotificationInteracted?.Invoke(this, CreateArgsFromInteractedEvent(args));
            }

            if (args.Arguments[NotificationIdKey] == Id)
            {
                _interacted = true;
                OnInteracted(CreateArgsFromInteractedEvent(args).WithNotification(this));
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

    /// <summary>
    /// Extends <see cref="WinUiNotification"/> with statically typed properties.
    /// </summary>
    /// <typeparam name="T">Properties type.</typeparam>
    public abstract class WinUiNotification<T> : WinUiNotification, INotification<T> where T : new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNotification{T}"/> class.
        /// </summary>
        /// <param name="registrationId">
        /// The identifier assigned during registration with the <see cref="NotificationManager"/>
        /// </param>
        public WinUiNotification(string registrationId) : base(registrationId) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNotification{T}"/> class.
        /// </summary>
        /// <param name="notificationId">The unique identifier for the notification.</param>
        /// <param name="registrationId">The identifier assigned during registration with the <see cref="NotificationManager"/>.</param>
        /// <param name="tag">An optional tag associated with the notification.</param>
        /// <param name="scheduledTime">The scheduled time for the notification.</param>
        protected WinUiNotification(string notificationId, string registrationId, string? tag, DateTimeOffset scheduledTime)
            : base(notificationId, registrationId, tag, scheduledTime) { }

        /// <inheritdoc/>
        public T Properties { get; set; } = new();
    }

    /// <summary>
    /// Extends <see cref="WinUiNotification{T}"/> with statically typed arguments of <see cref="WinUiNotification.Interacted"/> event.
    /// </summary>
    /// <typeparam name="TProps">Properties type.</typeparam>
    /// <typeparam name="TArgs">Interacted event arguments type.</typeparam>
    public abstract class WinUiNotification<TProps, TArgs> : WinUiNotification<TProps>, INotification<TProps, TArgs> where TProps : new()
    {
        private EventHandler<NotificationInteractedEventArgs<TArgs?>>? _interactedHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNotification{TProps, TArgs}"/> class.
        /// </summary>
        /// <param name="registrationId">
        /// The identifier assigned during registration with the <see cref="NotificationManager"/>
        /// </param>
        public WinUiNotification(string registrationId) : base(registrationId) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNotification{TProps, TArgs}"/> class.
        /// </summary>
        /// <param name="notificationId">The unique identifier for the notification.</param>
        /// <param name="registrationId">The identifier assigned during registration with the <see cref="NotificationManager"/>.</param>
        /// <param name="tag">An optional tag associated with the notification.</param>
        /// <param name="scheduledTime">The scheduled time for the notification.</param>
        protected WinUiNotification(string notificationId, string registrationId, string? tag, DateTimeOffset scheduledTime)
            : base(notificationId, registrationId, tag, scheduledTime) { }

        /// <inheritdoc/>
        protected override abstract NotificationInteractedEventArgs<TArgs> CreateArgsFromInteractedEvent(AppNotificationActivatedEventArgs args);

        /// <inheritdoc/>
        protected override void OnInteracted(NotificationInteractedEventArgs e)
        {
            base.OnInteracted(e);
            _interactedHandler?.Invoke(this, (e as NotificationInteractedEventArgs<TArgs>)!);
        }

        /// <inheritdoc/>
        event EventHandler<NotificationInteractedEventArgs<TArgs?>>? INotification<TProps, TArgs>.Interacted
        {
            add => _interactedHandler += value;
            remove => _interactedHandler -= value;
        }
    }
}
