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
                new("abc", NotificationState.Created),
                new("def", NotificationState.Created),
                new("ghi", NotificationState.Scheduled),
                new("jkl", NotificationState.Displayed),
                new("mno", NotificationState.Displayed),
                new("prs", NotificationState.Displayed),
                new("tuw", NotificationState.Interacted),
            };
        }

        public ObservableCollection<Notification> Notifications { get; } = new();
    }
}
