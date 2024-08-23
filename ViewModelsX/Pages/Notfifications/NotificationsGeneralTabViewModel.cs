using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Navigation;
using MochaCore.Notifications;
using MochaCore.Notifications.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelsX.Dialogs;

namespace ViewModelsX.Pages.Notfifications
{
    public partial class NotificationsGeneralTabViewModel : ObservableObject
    {
        private readonly INavigator _navigator;

        public NotificationsGeneralTabViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

        [ObservableProperty]
        private string _notificationText = string.Empty;

        [ObservableProperty]
        private string _notificationImagePath = string.Empty;

        [ObservableProperty]
        private bool _isDelayScheduleChecked;

        [ObservableProperty]
        private TimeSpan? _selectedScheduleTime;

        [RelayCommand]
        private async Task BrowseImage()
        {
            using IDialogModule<OpenFileDialogProperties> openFileDialogModule = AppDialogs.StandardOpenFileDialog.Module;
            openFileDialogModule.Properties.TrySetInitialDirectory(Environment.SpecialFolder.Desktop);
            openFileDialogModule.Properties.Filters = [
                new ExtensionFilter("Images", ["jpg", "jpeg", "png"]),
                ];
            bool? result = await openFileDialogModule.ShowModalAsync(_navigator.Module.View);
            if (result is true)
            {
                NotificationImagePath = openFileDialogModule.Properties.SelectedPaths.First();
            }
        }

        [RelayCommand]
        private void Schedule()
        {
            INotification<GeneralNotificationProperties> notification = NotificationManager.RetrieveNotification<GeneralNotificationProperties>("GeneralNotification");
            notification.Properties.Title = "Mocha general notification";
            notification.Properties.Content = $"Your text is: {NotificationText}";
            notification.Properties.LeftButton = "Yes";
            notification.Properties.RightButton = "No";
            notification.Properties.Image = NotificationImagePath;

            notification.Interacted += NotificationInteracted;

            notification.Schedule();
        }

        private void NotificationInteracted(object? sender, NotificationInteractedEventArgs e)
        {
            ((INotification)sender!).Interacted -= NotificationInteracted;
        }
    }
}
