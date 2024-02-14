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

        public bool SupressDispose { get; } //

        public object? RawArgs { get; }
    }
}
