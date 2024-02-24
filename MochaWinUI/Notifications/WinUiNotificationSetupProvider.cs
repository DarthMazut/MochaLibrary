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
            IReadOnlyDictionary<string, object> rawArgs = ParseRawArgs(args.Arguments, args.UserInput);
            _rawNotificationHandler!.Invoke(new RawNotificationInteractedArgs(rawArgs, id));
        }

        private IReadOnlyDictionary<string, object> ParseRawArgs(IDictionary<string, string> arguments, IDictionary<string, string> userInput)
        {
            Dictionary<string, object> rawArgs = new();
            foreach ((string key, string arg) in arguments)
            {
                rawArgs[key] = arg;
            }

            foreach ((string key, string input) in userInput)
            {
                if (rawArgs.TryGetValue(key, out object? value))
                {
                    rawArgs[key] = new List<string>() { (string)value, input };
                    continue;   
                }

                rawArgs[key] = input;
            }

            return rawArgs;
        }
    }
}
