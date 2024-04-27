using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dispatching;
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

        public Notification(INotification notification)
        {
            _notification = notification;
            Timer timer = new(OnTimerTick, default, default, 1000);
            notification.Interacted += (s, e) =>
            {
                DispatcherManager.GetMainThreadDispatcher().EnqueueOnMainThread(() =>
                {
                    State = NotificationState.Interacted;
                });
            };
        }

        [ObservableProperty]
        public string? _title;

        public string Id => _notification.Id;

        [ObservableProperty]
        public string? _tag;

        [ObservableProperty]
        private DateTimeOffset _scheduledTime;

        public string ScheduledTimeString => ScheduledTime.ToString("HH:mm:ss (dd MMM yyyy)");

        public string TimeRemaining
        { 
            get
            {
                TimeSpan timeRemaining = ScheduledTime - DateTimeOffset.Now;
                return timeRemaining >= TimeSpan.Zero ? timeRemaining.ToString("HH:mm:ss") : " - ";
            }
        }

        [ObservableProperty]
        private NotificationState _state;

        [RelayCommand]
        private void Schedule()
        {
            if (!_notification.IsDisplayed)
            {
                _notification.Schedule(ScheduledTime);
                State = NotificationState.Scheduled;
            }
        }

        partial void OnTagChanged(string? value)
        {
            if (!_notification.IsDisplayed)
            {
                _notification.Tag = value;
            }
        }

        private void OnTimerTick(object? state)
        {
            if (_notification.IsDisplayed && !_notification.IsInteracted)
            {
                State = NotificationState.Displayed;
            }

            OnPropertyChanged(nameof(TimeRemaining));
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
