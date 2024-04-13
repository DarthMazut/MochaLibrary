using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Notifications
{
    public class Notification
    {
        public Notification(string text, NotificationState state)
        {
            Text = text;
            State = state;
        }

        public string Text { get; }

        public NotificationState State { get; }
    }

    public enum NotificationState
    {
        Created,
        Scheduled,
        Displayed,
        Interacted
    }
}
