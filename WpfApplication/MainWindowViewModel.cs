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
            IDialogModule<StandardMessageDialogProperties> dialog = DialogManager.GetDialog<StandardMessageDialogProperties>("MessageBox");
            dialog.Properties.Title = "Title";
            dialog.Properties.Message = "Hello there!";
            await dialog.ShowModalAsync(this);
        }
    }
}