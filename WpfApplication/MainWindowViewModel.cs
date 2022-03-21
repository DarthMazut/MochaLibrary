using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
using MochaCore.Utils;
using System;
using System.Windows.Input;

namespace WpfApplication
{
    public class MainWindowViewModel
    {
        public ICommand ShowDialogCommand => new SimpleCommand(ShowDialog);

        public ICommand ShowBrowseDialogCommand => new SimpleCommand(ShowBrowseDialog);

        private async void ShowDialog(object? param)
        {
            ICustomDialogModule<DialogProperties> dialog = DialogManager.GetCustomDialog<DialogProperties>("MyDialog");
            dialog.Closing += async (s, e) =>
            {
                IDialogModule<StandardMessageDialogProperties> dialog = DialogManager.GetDialog<StandardMessageDialogProperties>("MessageBox");
                dialog.Properties.Title = "Confirmation";
                dialog.Properties.Message = "Are you sure you want quit current dialog?";
                dialog.Properties.DeclineButtonText = "No";
                bool? result = await dialog.ShowModalAsync(this);

                if (result != true)
                {
                    e.Cancel = true;
                }
            };

            await dialog.ShowModalAsync(this); 
        }

        private async void ShowBrowseDialog(object? obj)
        {
            IDialogModule<BrowseFolderDialogProperties> myBrowseDialog = DialogManager.GetDialog<BrowseFolderDialogProperties>("BrowseDialog");
            myBrowseDialog.Properties.Title = "Hello There!";
            myBrowseDialog.Properties.InitialDirectory = @"C:\Users\Sebastian Kasperczyk\Desktop\InstallTest\";
            bool? result = await myBrowseDialog.ShowModalAsync(this);
        }
    }
}