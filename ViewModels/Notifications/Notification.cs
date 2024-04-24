using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Notifications
{
    public partial class Notification : ObservableObject
    {
        private readonly INotification _notification;

        private bool _canMutate;

        public Notification(INotification notification)
        {
            _notification = notification;
            _canMutate = notification.ScheduledTime is not null || notification.IsDisplayed;
        }

        [ObservableProperty]
        public string? _title;

        public string Id => _notification.Id;

        [ObservableProperty]
        public string? _tag;

        [ObservableProperty]
        private DateTimeOffset _scheduledTime;

        public string ScheduledTimeString => ScheduledTime.ToString("HH:mm:ss (dd MMM yyyy)");

        [ObservableProperty]
        private NotificationState _state;

        [RelayCommand]
        private void Schedule()
        {
            if (!_notification.IsDisplayed)
            {
                _notification.Schedule(ScheduledTime);
            }
        }
    }

    public enum NotificationState
    {
        Created,
        Scheduled,
        Displayed,
        Interacted
    }
}
