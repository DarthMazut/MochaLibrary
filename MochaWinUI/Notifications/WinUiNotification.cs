using Microsoft.Windows.AppNotifications;
using MochaCore.Notifications;
using MochaWinUI.Notifications.Extensions;
using System;
using System.Collections.Generic;
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

        private string? _tag;
        private DateTimeOffset? _scheduledTime;
        private bool _displayed;
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
            ToastNotificationManager.CreateToastNotifier().ScheduledToastNotificationShowing += NotificationDisplayed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNotification"/> class.
        /// <para>
        /// TODO: explain when this constructor is needed.
        /// </para>
        /// </summary>
        /// <param name="notificationId">The unique identifier for the notification.</param>
        /// <param name="registrationId">The identifier assigned during registration with the <see cref="NotificationManager"/>.</param>
        /// <param name="tag">An optional tag associated with the notification.</param>
        /// <param name="scheduledTime">The scheduled time for the notification.</param>
        /// <param name="wasDisplayed">Whether this notification was already displayed and so cannot be scheduled again.</param>
        /// <param name="wasInteracted">Whether this notification was already interacted by the user.</param>
        protected WinUiNotification(
            string notificationId,
            string registrationId,
            string? tag,
            DateTimeOffset scheduledTime,
            bool wasDisplayed,
            bool wasInteracted)
        {
            Id = notificationId;
            RegistrationId = registrationId;
            Tag = tag;
            _scheduledTime = scheduledTime;
            _interacted = wasInteracted;
            _displayed = wasDisplayed;

            if (wasInteracted && !wasDisplayed)
            {
                throw new ArgumentException("Notification cannot be interacted and not displayed at the same time.");
            }

            if (!wasDisplayed)
            {
                ToastNotificationManager.GetDefault().CreateToastNotifier().ScheduledToastNotificationShowing += NotificationDisplayed;
            }

            if (!wasInteracted)
            {
                AppNotificationManager.Default.NotificationInvoked += AnyNotificationInvoked;
            }
        }

        /// <inheritdoc/>
        public string Id { get; }

        /// <summary>
        /// Unique identifier provided during registration with <see cref="NotificationManager"/>.
        /// </summary>
        public string RegistrationId { get; }

        /// <inheritdoc/>
        public DateTimeOffset? ScheduledTime => _scheduledTime;

        /// <inheritdoc/>
        public bool IsInteracted => _interacted;

        /// <inheritdoc/>
        public bool IsDisplayed => _displayed;

        /// <inheritdoc/>
        public bool IsDisposed => _isDisposed;

        /// <inheritdoc/>
        public string? Tag
        {
            get => _tag;
            set
            {
                if (IsDisplayed)
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
            Unschedule();

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
            ScheduleRemovalFromMessageCenter();
            _isDisposed = true;
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// When overriden should return definition of current instance as <see langword="string"/>.
        /// </summary>
        protected abstract string CreateNotificationDefinition();

        /// <summary>
        /// When overriden, should return new instance of <see cref="MochaCore.Notifications.INotification"/>
        /// that represents notification which interaction was described by provided arguments. 
        /// Created instance should have <see cref="MochaCore.Notifications.INotification.IsDisplayed"/> 
        /// and <see cref="MochaCore.Notifications.INotification.IsInteracted"/> set to <see langword="true"/>
        /// </summary>
        /// <param name="args">Describes notification interaction.</param>
        protected abstract INotification CreateInteractedNotification(AppNotificationActivatedEventArgs args);

        /// <summary>
        /// When overriden should return active instance of <see cref="INotification"/> representing
        /// provided <see cref="ScheduledToastNotification"/> instance.
        /// </summary>
        /// <param name="notification">Source of data for creating instance.</param>
        protected abstract INotification? CreatePendingNotification(ScheduledToastNotification notification);

        /// <summary>
        /// When overriden should return new instance of <see cref="MochaCore.Notifications.INotification"/>
        /// that represents provided <see cref="ToastNotification"/>. Created instance should have 
        /// <see cref="MochaCore.Notifications.INotification.IsDisplayed"/> set to <see langword="true"/> and
        /// <see cref="MochaCore.Notifications.INotification.IsInteracted"/> to <see langword="false"/>
        /// </summary>
        /// <param name="notification">Source of data for creating instance.</param>
        protected abstract INotification CreateDisplayedNotification(ToastNotification notification);

        /// <summary>
        /// When overriden should return new instance of suitable <see cref="NotificationInteractedEventArgs"/>.
        /// This basically means that when you implementing <see cref="INotification{TProps, TArgs}"/> you should
        /// return corresponding <see cref="NotificationInteractedEventArgs{T}"/> object, with matching generic type.
        /// </summary>
        /// <param name="args">Source of data for creating instance.</param>
        protected virtual NotificationInteractedEventArgs CreateArgsFromInteractedEvent(AppNotificationActivatedEventArgs args)
            => new (
                CreateInteractedNotification(args),
                args.Arguments[InvokedItemIdKey],
                args.AsDictionary(),
                args);

        /// <summary>
        /// Raises the <see cref="Interacted"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnInteracted(NotificationInteractedEventArgs e)
        {
            Interacted?.Invoke(this, e);
        }

        IReadOnlyCollection<INotification> INotificationSharedDataProvider.GetPendingNotifications()
            => ToastNotificationManager
                .GetDefault()
                .CreateToastNotifier()
                .GetScheduledToastNotifications()
                .Where(n => n.IsValid())
                .Select(n => CreatePendingNotification(n))
                .ToList().AsReadOnly()!;

        IReadOnlyCollection<INotification> INotificationSharedDataProvider.GetActionCenterNotifications()
            => ToastNotificationManager.History
                .GetHistory()
                .Where(n => n.IsValid())
                .Select(n => CreateDisplayedNotification(n))
                .ToList().AsReadOnly();

        private void Unschedule()
        {
            ScheduledToastNotification? scheduledNotification =
                ToastNotificationManager
                   .GetDefault()
                   .CreateToastNotifier()
                   .GetScheduledToastNotifications()
                   .FirstOrDefault(n => n.GetValueByKey(NotificationIdKey) == Id);

            if (scheduledNotification is not null)
            {
                ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(scheduledNotification);
            }
        }

        private void ScheduleRemovalFromMessageCenter()
        {
            _ = Task.Run(async () =>
            {
                IList<AppNotification> notifications = await AppNotificationManager.Default.GetAllAsync();
                AppNotification? currentNotification = notifications.FirstOrDefault(n => n.GetValueByKey(NotificationIdKey) == Id);
                await AppNotificationManager.Default.RemoveByIdAsync(currentNotification?.Id ?? default);
            });
        }

        private void ScheduleGuard()
        {
            if (IsDisplayed || IsDisposed)
            {
                throw new InvalidOperationException("Disposed or displayed notification cannot be rescheduled.");
            }
        }

        private void NotificationDisplayed(ToastNotifier sender, ScheduledToastNotificationShowingEventArgs e)
        {
            string? id = e.ScheduledToastNotification.GetValueByKey(NotificationIdKey);
            if (id == Id)
            {
                _displayed = true;
            }
        }

        private void AnyNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
        {
            if (!args.AreValid())
            {
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
    }

    /// <summary>
    /// Extends <see cref="WinUiNotification"/> with statically typed properties.
    /// </summary>
    /// <typeparam name="T">Properties type.</typeparam>
    public abstract class WinUiNotification<T> : WinUiNotification, INotificationRoot<T> where T : new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNotification{T}"/> class.
        /// </summary>
        /// <param name="registrationId">
        /// The identifier assigned during registration with the <see cref="NotificationManager"/>
        /// </param>
        public WinUiNotification(string registrationId) : base(registrationId) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNotification"/> class.
        /// <para>
        /// TODO: explain when this constructor is needed.
        /// </para>
        /// </summary>
        /// <param name="notificationId">The unique identifier for the notification.</param>
        /// <param name="registrationId">The identifier assigned during registration with the <see cref="NotificationManager"/>.</param>
        /// <param name="tag">An optional tag associated with the notification.</param>
        /// <param name="scheduledTime">The scheduled time for the notification.</param>
        /// <param name="wasDisplayed">Whether this notification was already displayed and so cannot be scheduled again.</param>
        /// <param name="wasInteracted">Whether this notification was already interacted by the user.</param>
        protected WinUiNotification(string notificationId, string registrationId, string? tag, DateTimeOffset scheduledTime, bool wasDisplayed, bool wasInteracted)
            : base(notificationId, registrationId, tag, scheduledTime, wasDisplayed, wasInteracted) { }

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
        /// Initializes a new instance of the <see cref="WinUiNotification"/> class.
        /// <para>
        /// TODO: explain when this constructor is needed.
        /// </para>
        /// </summary>
        /// <param name="notificationId">The unique identifier for the notification.</param>
        /// <param name="registrationId">The identifier assigned during registration with the <see cref="NotificationManager"/>.</param>
        /// <param name="tag">An optional tag associated with the notification.</param>
        /// <param name="scheduledTime">The scheduled time for the notification.</param>
        /// <param name="wasDisplayed">Whether this notification was already displayed and so cannot be scheduled again.</param>
        /// <param name="wasInteracted">Whether this notification was already interacted by the user.</param>
        protected WinUiNotification(string notificationId, string registrationId, string? tag, DateTimeOffset scheduledTime, bool wasDisplayed, bool wasInteracted)
            : base(notificationId, registrationId, tag, scheduledTime, wasDisplayed, wasInteracted) { }

        /// <summary>
        /// When overriden should return new instance of <c>TArgs</c> based on provided <paramref name="args"/>.
        /// </summary>
        /// <param name="args">Source of data for creating instance.</param>
        protected abstract TArgs CreateInteractionData(AppNotificationActivatedEventArgs args);

        /// <inheritdoc/>
        protected override void OnInteracted(NotificationInteractedEventArgs e)
        {
            base.OnInteracted(e);
            _interactedHandler?.Invoke(this, (e as NotificationInteractedEventArgs<TArgs>)!);
        }

        event EventHandler<NotificationInteractedEventArgs<TArgs?>>? INotification<TProps, TArgs>.Interacted
        {
            add => _interactedHandler += value;
            remove => _interactedHandler -= value;
        }

        /// <inheritdoc/>
        protected override sealed NotificationInteractedEventArgs<TArgs> CreateArgsFromInteractedEvent(AppNotificationActivatedEventArgs args)
            => new(
                CreateInteractedNotification(args),
                args.Arguments[InvokedItemIdKey],
                args.AsDictionary(),
                args,
                CreateInteractionData(args));
    }
}
