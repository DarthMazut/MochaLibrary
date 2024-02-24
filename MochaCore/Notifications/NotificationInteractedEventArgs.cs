using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Notifications
{
    public class NotificationInteractedEventArgs : EventArgs
    {
        public INotification Notification { get; }

        public string InvokedItemId { get; }

        public string? TextInput { get; }

        public string? SelectedItemId { get; }

        public DateTimeOffset? SlectedDate { get; }

        public IReadOnlyDictionary<string, object> RawArgs { get; }
    }
}
