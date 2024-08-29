using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Dispatching;
using MochaCore.Navigation;
using MochaCore.Notifications;
using MochaCore.Notifications.Extensions;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelsX.Dialogs;

namespace ViewModelsX.Pages.Notfifications
{
    public partial class NotificationsGeneralTabViewModel : ObservableObject, IOnNavigatedTo, IOnNavigatedFrom
    {
        private readonly INavigator _navigator;
        private readonly Dictionary<string, string> _notificationSelectableItems = new()
        {
            {"i1", "Item #1"},
            {"i2", "Item #2"},
            {"i3", "Item #3"}
        };

        public NotificationsGeneralTabViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

        public void OnNavigatedTo(OnNavigatedToEventArgs e)
        {
            SelectedScheduleTime = DateTime.Now.TimeOfDay;
            NotificationManager.NotificationInteracted += NotificationInteracted;

            if (e.Parameter is NotificationInteractedEventArgs args)
            {
                HandleNotificationInteraction(args);
            }
        }

        public void OnNavigatedFrom(OnNavigatedFromEventArgs e)
        {
            NotificationManager.NotificationInteracted -= NotificationInteracted;
        }

        [ObservableProperty]
        private string _notificationText = string.Empty;

        [ObservableProperty]
        private string _notificationImagePath = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanSchedule))]
        private bool _isDelayScheduleChecked;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanSchedule))]
        private TimeSpan? _selectedScheduleTime;

        public bool CanSchedule => !IsDelayScheduleChecked || IsInTheFuture(SelectedScheduleTime);

        [ObservableProperty]
        private string _result = string.Empty;

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
                ContentImage = @"C:\Users\AsyncMilk\Desktop\notify_test.png",
                Content = $"The text you provided was: {NotificationText}.{Environment.NewLine}" +
                $"You can add custom text into `TextBox` and select an example item from `ComboBox` " +
                $"- these values will be received by the application, after clicking one of available buttons.",
                HasTextInput = true,
                TextInputPlaceholder = "The text provided here will be handled by the running app.",
                SelectableItems = _notificationSelectableItems,
                SelectableItemsHeader = "Choose any item here (default is 2):",
                InitialSelectableItemId = "i2",
                LeftButton = "Left button",
                MiddleButton = "Middle button",
                RightButton = "Right button",
            };

            notification.Tag = GetType().ToString();

            try
            {
                if (IsDelayScheduleChecked)
                {
                    DateTimeOffset scheduleTime = DateTimeOffset.Now.Date + (SelectedScheduleTime ?? default);
                    notification.Schedule(scheduleTime);
                }
                else
                {
                    notification.Schedule();
                }
                
                Result = "Scheduled!";
            }
            catch (Exception ex)
            {
                Result = ex.ToString();
            }
        }

        private void NotificationInteracted(object? sender, NotificationInteractedEventArgs e)
        {
            DispatcherManager.GetMainThreadDispatcher().EnqueueOnMainThread(() =>
            {
                if (e.Notification.Tag == GetType().ToString())
                {
                    HandleNotificationInteraction(e);
                }
            });
        }

        private void HandleNotificationInteraction(NotificationInteractedEventArgs e)
        {
            (WindowManager.GetOpenedModules().First() as IWindowModule)?.Restore();
            Result = CreateResultFromArgs(e);
        }

        private bool IsInTheFuture(TimeSpan? selectedScheduleTime)
            => selectedScheduleTime > DateTime.Now.TimeOfDay;

        private string CreateResultFromArgs(NotificationInteractedEventArgs args)
            => $"Your text input was: {args.TextInput}{Environment.NewLine}" +
            $"Your selected item was: {args.SelectedItemId} : {_notificationSelectableItems.GetValueOrDefault(args.SelectedItemId ?? string.Empty)}{Environment.NewLine}" +
            $"You pressed the button: {args.InvokedItemId}";
    }
}
