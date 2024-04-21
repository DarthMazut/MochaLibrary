using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dispatching;
using MochaCore.Navigation;
using MochaCore.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Notifications
{
    public partial class NotificationsPageViewModel : ObservableObject, INavigationParticipant
    {
        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        public NotificationsPageViewModel()
        {
            Notifications = new()
            {
                new Notification()
                {
                    Title="Dupa",
                    Id = "dfdf",
                    State = NotificationState.Created,
                    Tag="sdsd",
                    ScheduledTime = DateTimeOffset.Now
                },
                new Notification()
                {
                    Title="MyNotification",
                    Id = "123456",
                    State = NotificationState.Created,
                    Tag="xyz",
                    ScheduledTime = DateTimeOffset.Now + TimeSpan.FromHours(2)
                },
            };

            _ = Task.Run(async () =>
            {
                await Task.Delay(7000);
                DispatcherManager.GetMainThreadDispatcher().RunOnMainThread(() =>
                {
                    Notifications[1].State = NotificationState.Scheduled;
                });
            });
        }

        public ObservableCollection<Notification> Notifications { get; } = new();
    }
}
