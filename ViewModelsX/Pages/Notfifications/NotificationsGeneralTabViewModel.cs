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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelsX.Dialogs;
using ViewModelsX.Pages.Notfifications.Dialogs;

namespace ViewModelsX.Pages.Notfifications
{
    public partial class NotificationsGeneralTabViewModel : ObservableObject, IOnNavigatedTo, IOnNavigatedFrom
    {
        public static readonly string NOTIFICATION_TAG = "GeneralNotificationTag";
        
        private readonly INavigator _navigator;

        public NotificationsGeneralTabViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

        public void OnNavigatedTo(OnNavigatedToEventArgs e)
        {

        }

        public void OnNavigatedFrom(OnNavigatedFromEventArgs e)
        {

        }

        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private string? _content;

        [ObservableProperty]
        private string? _notificationImagePath;

        [ObservableProperty]
        private string? _contentImagePath;

        [ObservableProperty]
        private bool _isTextInputChecked;

        [ObservableProperty]
        private string? _textInputHeader;

        [ObservableProperty]
        private string? _textInputPlaceholder;

        [ObservableProperty]
        private bool _isSelectableItemChecked;

        [ObservableProperty]
        private string? _selectableItemHeader;

        [ObservableProperty]
        private ObservableCollection<SelectableItem> _selectableItems = [ new("i1", "Item #1"), new("i2", "Item #2"), new("i3", "Item #3")];

        [ObservableProperty]
        private bool _isLeftButtonChecked;

        [ObservableProperty]
        private string? _leftButtonText;

        [ObservableProperty]
        private bool _isMiddleButtonChecked;

        [ObservableProperty]
        private string? _middleButtonText;

        [ObservableProperty]
        private bool _isRightButtonChecked;

        [ObservableProperty]
        private string? _rightButtonText;

        [ObservableProperty]
        private bool _isDelayChecked;

        [ObservableProperty]
        private double _delayValue;

        [ObservableProperty]
        private ObservableCollection<InteractionItem> _interactionData = [ new("Test", "Value"), new("Test 2", "Other value")];

        [RelayCommand]
        private Task BrowseNotificationImage() => BrowseImageCore((paths) => NotificationImagePath = paths.First());

        [RelayCommand]
        private Task BrowseContentImage() => BrowseImageCore((paths) => ContentImagePath = paths.First());

        [RelayCommand]
        private void RemoveItem(SelectableItem item) => SelectableItems.Remove(item);

        [RelayCommand]
        private async Task AddItem()
        {
            using ICustomDialogModule<AddSelectableItemDialogProperties> dialogModule = AppDialogs.AddSelectableItemDialog.Module;
            if (await dialogModule.ShowModalAsync(_navigator.Module.View) is true)
            {
                if (dialogModule.Properties.CreatedItem is SelectableItem item)
                {
                    SelectableItems.Add(item);
                } 
            }
        }

        [RelayCommand]
        private void Schedule()
        {
            INotification<GeneralNotificationProperties> notification = NotificationManager.RetrieveNotification<GeneralNotificationProperties>("GeneralNotification");
            notification.Properties = new GeneralNotificationProperties()
            {
                Title = !string.IsNullOrEmpty(Title) ? Title : null,
                Content = !string.IsNullOrEmpty(Content) ? Content : null,
                NotificationImage = !string.IsNullOrEmpty(NotificationImagePath) ? NotificationImagePath : null,
                ContentImage = !string.IsNullOrEmpty(ContentImagePath) ? ContentImagePath : null,
                HasTextInput = IsTextInputChecked,
                TextInputHeader = !string.IsNullOrEmpty(TextInputHeader) ? TextInputHeader : null,
                TextInputPlaceholder = !string.IsNullOrEmpty(TextInputPlaceholder) ? TextInputPlaceholder : null,
                SelectableItemsHeader = !string.IsNullOrEmpty(SelectableItemHeader) ? SelectableItemHeader : null,
                SelectableItems = IsSelectableItemChecked ? SelectableItems.ToDictionary(i => i.Key, i => i.Name) : null,
                LeftButton = IsLeftButtonChecked ? (LeftButtonText ?? string.Empty) : null,
                MiddleButton = IsMiddleButtonChecked ? (MiddleButtonText ?? string.Empty) : null,
                RightButton = IsRightButtonChecked ? (RightButtonText ?? string.Empty) : null
            };

            notification.Tag = NOTIFICATION_TAG;

            try
            {
                if (IsDelayChecked)
                {
                    DateTimeOffset scheduleTime = DateTimeOffset.Now.Date + TimeSpan.FromSeconds(DelayValue);
                    notification.Schedule(scheduleTime);
                }
                else
                {
                    notification.Schedule();
                }
                
                //Result = "Scheduled!";
            }
            catch (Exception ex)
            {
                //Result = ex.ToString();
            }
        }

        private void NotificationInteracted(object? sender, NotificationInteractedEventArgs e)
        {
            DispatcherManager.GetMainThreadDispatcher().EnqueueOnMainThread(() =>
            {
                if (e.Notification.Tag == GetType().ToString())
                {
                    //HandleNotificationInteraction(e);
                }
            });
        }

        private bool IsInTheFuture(TimeSpan? selectedScheduleTime)
            => selectedScheduleTime > DateTime.Now.TimeOfDay;

        //private string CreateResultFromArgs(NotificationInteractedEventArgs args)
        //    => $"Your text input was: {args.TextInput}{Environment.NewLine}" +
        //    $"Your selected item was: {args.SelectedItemId} : {_notificationSelectableItems.GetValueOrDefault(args.SelectedItemId ?? string.Empty)}{Environment.NewLine}" +
        //    $"You pressed the button: {args.InvokedItemId}";

        private async Task BrowseImageCore(Action<IList<string>> assignAction)
        {
            using IDialogModule<OpenFileDialogProperties> openFileDialogModule = AppDialogs.StandardOpenFileDialog.Module;
            openFileDialogModule.Properties.TrySetInitialDirectory(Environment.SpecialFolder.Desktop);
            openFileDialogModule.Properties.Filters = [
                new ExtensionFilter("Images", ["jpg", "jpeg", "png"]),
                ];
            bool? result = await openFileDialogModule.ShowModalAsync(_navigator.Module.View);
            if (result is true)
            {
                assignAction?.Invoke(openFileDialogModule.Properties.SelectedPaths);
            }
        }

        public record SelectableItem(string Key, string Name);

        public record InteractionItem(string Name, string Value);
    }
}
