using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
using MochaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WinUIApplication
{
    public class MainWindowViewModel
    {
        public ICommand OpenDialogCommand => new SimpleCommand((o) =>
        {
            IDialogModule<StandardMessageDialogProperties> messageDialog = DialogManager.GetDialog<StandardMessageDialogProperties>("Dialog1");
            messageDialog.Properties.Title = "Hello There!";
            messageDialog.Properties.Message = "Our long awaited meeting has come at last!";
            messageDialog.Properties.ConfirmationButtonText = "General Kenobi!";
            messageDialog.Properties.DeclineButtonText = "Nah!";
            messageDialog.ShowModalAsync(this);
        });
    }
}
