using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dispatching;
using MochaCore.Navigation;
using MochaCore.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public partial class NotificationsPageViewModel : ObservableObject, INavigationParticipant
    {
        public INavigator Navigator => MochaCore.Navigation.Navigator.Create();

        [ObservableProperty]
        private string _title = Pages.NotificationsPage.Name;

        [RelayCommand]
        private void ShowNotification()
        {
            INotification<object> notification = NotificationManager.RetrieveNotification<object>("MyNotification");
            //notification.Interacted += (s, e) =>
            //{
            //    DispatcherManager.GetMainThreadDispatcher().RunOnMainThread(() =>
            //    {
            //        Title = "Handled 😎";
            //    });
            //};
            notification.Schedule();
        }
    }
}
