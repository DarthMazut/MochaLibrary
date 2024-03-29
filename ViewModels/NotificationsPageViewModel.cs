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

        private INotification? _notification;

        [RelayCommand]
        private void ShowNotification()
        {
            if (_notification is null)
            {
                _notification = NotificationManager.RetrieveNotification("MyNotification");
                _notification.Tag = "MyTestTag :)";

                _notification.Interacted += (s, e) =>
                {
                    DispatcherManager.GetMainThreadDispatcher().RunOnMainThread(() =>
                    {
                        Title = "Handled 😎";
                        _notification = null;
                    });
                };
            }

            _notification.Schedule(DateTimeOffset.UtcNow + TimeSpan.FromSeconds(10));
        }
    }
}
