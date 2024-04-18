using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Notifications
{
    public class Notification
    {
        public string? Title { get; init; }

        public string? Id { get; init; }

        public string? Tag { get; init; }

        public DateTimeOffset ScheduledTime { get; init; }

        public string ScheduledTimeString => ScheduledTime.ToString("HH:mm:ss (dd MMM yyyy)");

        public NotificationState State { get; init; }
    }

    public enum NotificationState
    {
        Created,
        Scheduled,
        Displayed,
        Interacted
    }
}
