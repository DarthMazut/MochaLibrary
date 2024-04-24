using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
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

        public TimeSpan NowTime => TimeSpan.FromHours(NowDate.Hour) + TimeSpan.FromMinutes(NowDate.Minute);

        public DateTimeOffset NowDate => DateTimeOffset.Now;

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
            DialogControl.Properties.CustomProperties["Notification"] = SelectedSchema!.CreateNotification();
        }

        [RelayCommand]
        private void Close()
        {
            DialogControl.Close(default);
        }
    }
}
