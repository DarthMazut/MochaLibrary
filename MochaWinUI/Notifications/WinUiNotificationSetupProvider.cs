using Microsoft.Windows.AppNotifications;
using MochaCore.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.Notifications
{
    /// <summary>
    /// Sets up notifications for WinUI projects.
    /// </summary>
    public class WinUiNotificationSetupProvider : INotificationSetupProvider, IDisposable
    {
        private bool _isDisposed = false;
        private Action<RawNotificationInteractedArgs>? _rawNotificationHandler;

        /// <inheritdoc/>
        public void Setup(Action<RawNotificationInteractedArgs> rawNotificationHandler)
        {
            _rawNotificationHandler = rawNotificationHandler;

            AppNotificationManager.Default.NotificationInvoked += NotificationInvoked;
            AppNotificationManager.Default.Register();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            AppNotificationManager.Default.Unregister();
        }

        private void NotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
        {
            // TODO extract "Id" to some constant...
            _ = args.Arguments.TryGetValue("id", out string? id);
            _rawNotificationHandler!.Invoke(new RawNotificationInteractedArgs(id));
        }
    }
}
