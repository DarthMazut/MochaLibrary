using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
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

        [ObservableProperty]
        private ObservableCollection<Notification> _notifications = new();

        [RelayCommand]
        private async Task Loaded()
        {
            IReadOnlyCollection<INotification> pendingNotifications = await NotificationManager.GetPendingNotifications();
            IReadOnlyCollection<INotification> displayedNotifications = await NotificationManager.GetDisplayedNotifications();
            IReadOnlyCollection<INotification> createdNotifications = NotificationManager.GetCreatedNotifications();
            Notifications = new ObservableCollection<Notification>(pendingNotifications
                .Concat(displayedNotifications)
                .Concat(createdNotifications)
                .DistinctBy(n => n.Id)
                .Select(n => new Notification(n))
                .OrderBy(n => (int)n.State)
                .ToList());
        }

        [RelayCommand]
        private void DisposeNotification(Notification notification)
        {
            Notifications.Remove(notification);
            notification.Dispose();
        }

        [RelayCommand]
        private async Task AddNotification()
        {
            using ICustomDialogModule<DialogProperties> addNotificationDialog 
                = DialogManager.GetCustomDialog<DialogProperties>(Dialogs.EditNotificationDialog.ID);

            if (await addNotificationDialog.ShowModalAsync(this) is true)
            {
                Notification? createdNotification = addNotificationDialog.Properties.CustomProperties["Notification"] as Notification;
                if (createdNotification is not null)
                {
                    int targetIndex = Math.Max(Notifications.ToList().FindLastIndex(n => n.State == createdNotification.State), 0);
                    Notifications.Insert(targetIndex, createdNotification);
                }
            }
        }
    }
}
