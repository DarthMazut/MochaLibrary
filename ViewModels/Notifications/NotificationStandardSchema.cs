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
using System.Windows.Input;

namespace ViewModels.Notifications
{
    public partial class NotificationStandardSchema : ObservableObject, INotificationSchema
    {
        private readonly IDialogModule<OpenFileDialogProperties> _openFileDialog;

        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private string? _content;

        [ObservableProperty]
        private string? _imageUri;
        
        [RelayCommand]
        private async Task OpenFileDialog()
        {
            if (await _openFileDialog.ShowModalAsync(this) == true)
            {
                ImageUri = _openFileDialog.Properties.SelectedPaths.LastOrDefault();
            }
        }

        public NotificationStandardSchema(IDialogModule<OpenFileDialogProperties> openFileDialog)
        {
            _openFileDialog = openFileDialog;
        }

        public INotification CreateNotification()
        {
            INotification<GeneralNotificationProperties> notification 
                = NotificationManager.RetrieveNotification<GeneralNotificationProperties>("SimpleNotification");

            notification.Properties = new GeneralNotificationProperties()
            {
                Title = Title,
                Content = Content,
                Image = ImageUri
            };

            return notification;
        }

        public override string ToString() => "Standard schema";
    }
}
