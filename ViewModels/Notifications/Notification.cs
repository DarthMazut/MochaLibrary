using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Notifications
{
    public partial class Notification : ObservableObject
    {
        public string? Title { get; init; }

        public string? Id { get; init; }

        public string? Tag { get; init; }

        public DateTimeOffset ScheduledTime { get; init; }

        public string ScheduledTimeString => ScheduledTime.ToString("HH:mm:ss (dd MMM yyyy)");

        [ObservableProperty]
        private NotificationState _state;
    }

    public enum NotificationState
    {
        Created,
        Scheduled,
        Displayed,
        Interacted
    }
}
