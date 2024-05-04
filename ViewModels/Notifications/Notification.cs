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
        private readonly Timer? _timer;

        public Notification(INotification notification)
        {
            _notification = notification;
            Title = $"Notification {notification.Id.Split("-").LastOrDefault()}";
            Tag = notification.Tag;
            State = ResolveState(notification);
            ScheduledTime = notification.ScheduledTime ?? DateTimeOffset.Now;
            if (State != NotificationState.Displayed && State != NotificationState.Interacted)
            {
                _timer = new Timer(OnTimerTick, default, default, 1000);
            }

            notification.Interacted += (s, e) =>
            {
                DispatcherManager.GetMainThreadDispatcher().EnqueueOnMainThread(() =>
                {
                    State = NotificationState.Interacted;
                });
            };

            notification.Disposed += (s, e) =>
            {
                _timer?.Dispose();
            };
        }

        [ObservableProperty]
        public string? _title;

        public string Id => _notification.Id;

        [ObservableProperty]
        public string? _tag;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanSchedule))]
        [NotifyCanExecuteChangedFor(nameof(ScheduleCommand))]
        private DateTimeOffset _scheduledTime;

        public string ScheduledTimeString => ScheduledTime.ToString("HH:mm:ss (dd MMM yyyy)");

        public string TimeRemaining
        { 
            get
            {
                TimeSpan timeRemaining = ScheduledTime - DateTimeOffset.Now;
                return timeRemaining >= TimeSpan.Zero ? $"{timeRemaining:hh\\:mm\\:ss}" : " - ";
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanSchedule))]
        [NotifyCanExecuteChangedFor(nameof(ScheduleCommand))]
        private NotificationState _state;

        public bool CanSchedule => State == NotificationState.Created && ScheduledTime > DateTimeOffset.Now;

        [RelayCommand(CanExecute = nameof(CanSchedule))]
        private void Schedule()
        {
            if (!_notification.IsDisplayed)
            {
                _notification.Schedule(ScheduledTime);
                State = NotificationState.Scheduled;
            }
        }

        public void Dispose() => _notification.Dispose();

        partial void OnTagChanged(string? value)
        {
            if (!_notification.IsDisplayed)
            {
                _notification.Tag = value;
            }
        }

        private NotificationState ResolveState(INotification notification)
        {
            if (notification.IsInteracted) { return NotificationState.Interacted; }
            if (notification.IsDisplayed) { return NotificationState.Displayed; }
            if (notification.ScheduledTime is not null) { return NotificationState.Scheduled; }
            return NotificationState.Created;
        }

        private void OnTimerTick(object? state)
        {
            DispatcherManager.GetMainThreadDispatcher().EnqueueOnMainThread(() =>
            {
                if (_notification.IsDisplayed && !_notification.IsInteracted)
                {
                    State = NotificationState.Displayed;
                }

                OnPropertyChanged(nameof(TimeRemaining));
            });
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
