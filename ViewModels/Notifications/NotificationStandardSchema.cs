using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Notifications;
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
            throw new NotImplementedException();
        }

        public override string ToString() => "Standard schema";
    }
}
