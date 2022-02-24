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

        private async void ShowDialog(object? param)
        {
            IUserDialogModule<DialogProperties> dialog = DialogManager.GetUserDialog<DialogProperties>("MyDialog");
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
    }
}