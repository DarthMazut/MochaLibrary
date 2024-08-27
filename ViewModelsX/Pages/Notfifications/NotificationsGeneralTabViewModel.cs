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
    public partial class NotificationsGeneralTabViewModel : ObservableObject, IOnNavigatedTo
    {
        private readonly INavigator _navigator;

        public NotificationsGeneralTabViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

        public void OnNavigatedTo(OnNavigatedToEventArgs e)
        {
            if (e.Parameter is NotificationInteractedEventArgs args)
            {
                NotificationText = args.InvokedItemId;
            }
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
            notification.Properties = new GeneralNotificationProperties()
            {
                Title = "This is example notification",
                NotificationImage = string.IsNullOrWhiteSpace(NotificationImagePath) ? null : NotificationImagePath,
                Content = $"The text you provided was: {NotificationText}.{Environment.NewLine}" +
                $"You can add custom text into `TextBox` and select an example item from `ComboBox` " +
                $"- these values will be received by the application, after clicking one of available buttons.",
                HasTextInput = true,
                TextInputPlaceholder = "The text provided here will be handled by the running app.",
                SelectableItems = new Dictionary<string, string>()
                {
                    {"i1", "Item #1"},
                    {"i2", "Item #2"},
                    {"i3", "Item #3"}
                },
                SelectableItemsHeader = "Choose any item here (default is 2):",
                InitialSelectableItemId = "i2",
                LeftButton = "Left button",
                MiddleButton = "Middle button",
                RightButton = "Right button"

            };

            notification.Interacted += NotificationInteracted;
            notification.Schedule();
        }

        private void NotificationInteracted(object? sender, NotificationInteractedEventArgs e)
        {
            // Not on ui thread !
            //((INotification)sender!).Interacted -= NotificationInteracted;
        }
    }
}
