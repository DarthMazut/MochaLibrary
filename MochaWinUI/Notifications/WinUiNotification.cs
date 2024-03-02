using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using MochaCore.Notifications;
using System;
using System.Collections.Generic;
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
        private static readonly string TAG = "tag";

        private readonly string _registrationId;
        private readonly Action<AppNotificationInteractedEventArgs>? _generalHandler;

        private DateTimeOffset? _scheduledTime;
        private bool _displayed;
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNotification{T}"/> class.
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="generalHandler"></param>
        public WinUiNotification(string registrationId, Action<AppNotificationInteractedEventArgs>? generalHandler)
        {
            _registrationId = registrationId;
            _generalHandler = generalHandler;
            Id = Guid.NewGuid().ToString();
            AppNotificationManager.Default.NotificationInvoked += AnyNotificationInvoked;
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
            .AddArgument("id", ResolveId())
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

        public void Schedule(DateTimeOffset scheduledTime, string tag)
        {
            throw new NotImplementedException();
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
            if (args.Arguments["registrationdId"] == _registrationId)
            {
                _generalHandler?.Invoke(new AppNotificationInteractedEventArgs<T>("", "", new Dictionary<string, object>(), "", new()));
            }

            if (args.Arguments["id"] == ResolveId())
            {
                _displayed = true;
                Interacted?.Invoke(this, new NotificationInteractedEventArgs(this, "", new Dictionary<string, object>(), args));
                Dispose();
            }
        }
    }

}
