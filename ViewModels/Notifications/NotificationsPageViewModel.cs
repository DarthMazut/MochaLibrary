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

        public ObservableCollection<Notification> Notifications { get; } = new();

        [RelayCommand]
        private async Task AddNotification()
        {
            using ICustomDialogModule<DialogProperties> addNotificationDialog 
                = DialogManager.GetCustomDialog<DialogProperties>(Dialogs.EditNotificationDialog.ID);

            if (await addNotificationDialog.ShowModalAsync(this) is true)
            {
                INotification? createdNotification = addNotificationDialog.Properties.CustomProperties["Notification"] as INotification;
                if (createdNotification is not null)
                {
                    Notifications.Add(new Notification(createdNotification));
                }
            }
        }
    }
}
