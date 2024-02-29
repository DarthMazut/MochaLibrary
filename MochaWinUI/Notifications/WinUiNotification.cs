﻿using Microsoft.Windows.AppNotifications;
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
    public class WinUiNotification<T> : INotificationTracker, INotification<T> where T : new()
    {
        private readonly string _internalId = Guid.NewGuid().ToString();
        private EventHandler<NotificationInteractedEventArgs>? genericEvent;

        private DateTimeOffset? _scheduledTime;
        private bool _displayed;
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUiNotification{T}"/> class.
        /// </summary>
        public WinUiNotification()
        {
            AppNotificationManager.Default.NotificationInvoked += AnyNotificationInvoked;
        }

        /// <inheritdoc/>
        event EventHandler<NotificationInteractedEventArgs>? INotificationTracker.GenericNotificationInteracted
        {
            add => genericEvent += value;
            remove => genericEvent -= value;
        }

        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public T Properties { get; set; } = new();
        
        /// <inheritdoc/>
        public DateTimeOffset? ScheduledTime => _scheduledTime;

        /// <inheritdoc/>
        public bool Displayed => _displayed;

        /// <inheritdoc/>
        public bool IsDisposed => _isDisposed;

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

        /// <inheritdoc/>
        public void Schedule(DateTimeOffset scheduledTime)
        {
            _scheduledTime = scheduledTime;
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
            if (args.Arguments["id"] == ResolveId())
            {
                _displayed = true;
                Interacted?.Invoke(this, new NotificationInteractedEventArgs(this, "", new Dictionary<string, object>(), args));
                Dispose();
            }
        }

        private string ResolveId() => Id ?? _internalId;
    }

}
