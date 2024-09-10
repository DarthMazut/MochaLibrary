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
    public partial class NotificationsGeneralTabViewModel : ObservableObject, IOnNavigatedToAsync, IOnNavigatedFrom
    {
        public static readonly string NOTIFICATION_TAG = "GeneralNotificationTag";
        
        private readonly INavigator _navigator;

        private INotification<GeneralNotificationProperties>? _notification;
        private CancellationTokenSource? _cts;

        public NotificationsGeneralTabViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

        public async Task OnNavigatedToAsync(OnNavigatedToEventArgs e)
        {
            _notification = (await NotificationManager.GetPendingNotifications())
                .Where(n => n.Tag?.Contains(NOTIFICATION_TAG) == true)
                .FirstOrDefault() as INotification<GeneralNotificationProperties>;

            if (_notification is not null)
            {
                _notification.Interacted += NotificationInteracted;
                if (_notification.ScheduledTime is not null)
                {
                    ClearCts(true);
                    DateTimeOffset now = DateTimeOffset.Now;
                    TimeSpan delayValue = TimeSpan.FromSeconds(int.Parse(_notification.Tag!.Split(';')[1]));

                    _ = CreateDelayTask(
                        _notification.ScheduledTime.Value - now,
                        new Progress<TimeSpan>(t => DelayProgress = (now - _notification.ScheduledTime.Value + delayValue + t) / delayValue),
                        _cts!.Token).ContinueWith(t =>
                            DispatcherManager.GetMainThreadDispatcher().EnqueueOnMainThread(() => CanSchedule = true));
                }
                return;
            }

            if (e.Parameter is NotificationInteractedEventArgs args)
            {
                HandleNotificationInteraction(args);
            }

            CanSchedule = true;
        }

        public void OnNavigatedFrom(OnNavigatedFromEventArgs e)
        {
            if (_notification is not null)
            {
                _notification.Interacted -= NotificationInteracted;
            }

            ClearCts();   
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
        private bool _canSchedule;

        [ObservableProperty]
        private double _delayProgress;

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
        private async Task ScheduleOrAbort()
        {
            if (!CanSchedule)
            {
                Abort();
            }
            else
            {
                await Schedule();
            }
        }

        private async Task Schedule()
        {
            _notification = NotificationManager.RetrieveNotification<GeneralNotificationProperties>("GeneralNotification");
            _notification.Properties = new GeneralNotificationProperties()
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

            _notification.Tag = $"{NOTIFICATION_TAG};{DelayValue}";
            _notification.Interacted += NotificationInteracted;
            ClearCts(true);

            try
            {
                if (IsDelayChecked)
                {
                    DateTimeOffset scheduleTime = DateTimeOffset.Now + TimeSpan.FromSeconds(DelayValue);
                    _notification.Schedule(scheduleTime);
                    CanSchedule = false;

                    _ = CreateDelayTask(TimeSpan.FromSeconds(DelayValue),
                        new Progress<TimeSpan>(t => DelayProgress = t / TimeSpan.FromSeconds(DelayValue)),
                        _cts!.Token).ContinueWith(t =>
                            DispatcherManager.GetMainThreadDispatcher().EnqueueOnMainThread(() => CanSchedule = true));
                }
                else
                {
                    _notification.Schedule();
                }


            }
            catch (Exception ex) // Do not catch general exceptions :O
            {
                using ICustomDialogModule<StandardMessageDialogProperties> dialogModule = AppDialogs.StandardMessageDialog.Module;
                dialogModule.Properties = new StandardMessageDialogProperties()
                {
                    Title = "Something went wrong 😐",
                    Message = ex.ToString(),
                    ConfirmationButtonText = "Oh, well..."
                };
                await dialogModule.ShowModalAsync(_navigator.Module.View);
            }
        }

        private void Abort()
        {
            _notification?.Dispose();
            ClearCts();
            CanSchedule = true;
        }

        private void NotificationInteracted(object? sender, NotificationInteractedEventArgs e)
        {
            DispatcherManager.GetMainThreadDispatcher().EnqueueOnMainThread(() =>
            {
                if (e.Notification.Tag?.Contains(NOTIFICATION_TAG) == true)
                {
                    HandleNotificationInteraction(e);
                }
            });
        }

        private void HandleNotificationInteraction(NotificationInteractedEventArgs e)
            => InteractionData = [.. e.AsDictionary.Select(kvp => new InteractionItem(kvp.Key, kvp.Value?.ToString() ?? "NULL"))];

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

        private static async Task CreateDelayTask(TimeSpan delayTime, IProgress<TimeSpan> progress, CancellationToken cancellationToken = default, int intervalMs = 100)
        {
            TimeSpan currentProgress = TimeSpan.Zero;
            while (currentProgress < delayTime)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                if (delayTime - currentProgress < TimeSpan.FromMilliseconds(intervalMs))
                {
                    progress.Report(currentProgress);
                    await Task.Delay(delayTime - currentProgress);
                    progress.Report(currentProgress);
                    return;
                }

                progress.Report(currentProgress);
                await Task.Delay(intervalMs);
                progress.Report(currentProgress);
                currentProgress += TimeSpan.FromMilliseconds(intervalMs);
            }

            progress.Report(currentProgress);
        }

        private void ClearCts(bool createNew = false)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            if (createNew)
            {
                _cts = new();
            }
        }

        public record SelectableItem(string Key, string Name);

        public record InteractionItem(string Name, string Value);
    }
}
