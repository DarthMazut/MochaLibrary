using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Notifications;
using MochaCore.Notifications.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Notifications;

namespace ViewModels.DialogsVMs
{
    public partial class EditNotificationDialogViewModel : ObservableObject, ICustomDialog<DialogProperties>
    {
        public CustomDialogControl<DialogProperties> DialogControl { get; } = new();

        public EditNotificationDialogViewModel()
        {
            Time = TimeSpan.FromHours(Date.Hour) + TimeSpan.FromMinutes(Date.Minute);
        }

        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _tag;

        [ObservableProperty]
        private TimeSpan _time;

        [ObservableProperty]
        private DateTimeOffset _date = DateTimeOffset.Now;

        [ObservableProperty]
        private bool _scheduleOnClose;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanCreate))]
        [NotifyCanExecuteChangedFor(nameof(CreateCommand))]
        public INotificationSchema? _selectedSchema;

        public List<INotificationSchema> NotificationSchemas { get; } = new()
        {
            new NotificationStandardSchema(DialogManager.GetDialog<OpenFileDialogProperties>(Dialogs.SelectFileDialog.ID)),
            new NotificationHeroImageSchema()
        };

        public bool CanCreate => SelectedSchema is not null;


        [RelayCommand(CanExecute = nameof(CanCreate))]
        private void Create()
        {
            INotification coreNotification = SelectedSchema!.CreateNotification();
            Notification notification = new(coreNotification)
            {
                Title = Name,
                Tag = Tag,
                ScheduledTime = Date//new DateTimeOffset(
                    //Date.Year,
                    //Date.Month,
                    //Date.Day,
                    //0, 0, 0, Date.Offset) + Time
            };

            DialogControl.Properties.CustomProperties["Notification"] = notification;
            if (ScheduleOnClose)
            {
                notification.ScheduleCommand.Execute(default);
            }

            DialogControl.Close(true);
        }

        [RelayCommand]
        private void Close()
        {
            DialogControl.Close(false);
        }
    }
}
