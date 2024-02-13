using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using MochaCore.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.Notifications
{
    public class WinUiNotification<T> : INotification<T> where T : new()
    {
        private readonly string _internalId = Guid.NewGuid().ToString();

        public WinUiNotification()
        {
            AppNotificationManager.Default.NotificationInvoked += AnyNotificationInvoked;
        }

        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public T Properties { get; set; } = new();
        
        /// <inheritdoc/>
        public event EventHandler? Interacted;

        /// <inheritdoc/>
        public void Schedule()
        {
            AppNotification appNotification = new AppNotificationBuilder()
            .AddArgument("id", ResolveId())
            .AddText("Test!!!")
            .AddButton(new AppNotificationButton("Test Button"))
            .BuildNotification();

            AppNotificationManager.Default.Show(appNotification);
        }

        /// <inheritdoc/>
        public void Schedule(DateTimeOffset scheduledTime)
        {
            throw new NotImplementedException();
        }

        private void AnyNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
        {
            if (args.Arguments["id"] == ResolveId())
            {
                Interacted?.Invoke(this, EventArgs.Empty);
            }
        }

        private string ResolveId() => Id ?? _internalId;
    }

}
