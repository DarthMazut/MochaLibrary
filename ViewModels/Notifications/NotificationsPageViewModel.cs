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
        public INavigator Navigator => MochaCore.Navigation.Navigator.Create();

        public NotificationsPageViewModel()
        {
            Notifications = new()
            {
                new()
                { 
                    Id = "dfdf",
                    State = NotificationState.Created,
                    Tag="sdsd",
                    Title="Dupa",
                    ScheduledTime = DateTimeOffset.Now
                },
            };
        }

        public ObservableCollection<Notification> Notifications { get; } = new();
    }
}
