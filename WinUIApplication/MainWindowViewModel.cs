using Microsoft.UI.Xaml;
using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
using MochaCore.Events;
using MochaCore.Events.Extensions;
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
        public ICommand OnLoadedCommand => new SimpleCommand((o) =>
        {
            IEventProvider<AppClosingEventArgs> closeEventProvider = AppEventManager.RequestEventProvider<AppClosingEventArgs>("OnClosing");
            closeEventProvider.SubscribeAsync(new AsyncEventHandler<AppClosingEventArgs>(myAsyncOnClosingEvent));
        });

        public ICommand OpenDialogCommand => new SimpleCommand(async (o) =>
        {
            IDialogModule<StandardMessageDialogProperties> dlg = DialogManager.GetDialog<StandardMessageDialogProperties>("Dialog1");
            _ = await dlg.ShowModalAsync(this);
        });

        private async Task myAsyncOnClosingEvent(AppClosingEventArgs e, IReadOnlyCollection<AsyncEventHandler> collection)
        {
            IDialogModule<StandardMessageDialogProperties> exitDialog = DialogManager.GetDialog<StandardMessageDialogProperties>("Dialog1");
            StandardMessageDialogProperties properties = new();
            properties.Title = "Confirm app close";
            properties.Message = "Are you sure you want to quit app now?";
            properties.ConfirmationButtonText = "Yes";
            properties.DeclineButtonText = "No";
            properties.CancelButtonText = "Cancel";
            exitDialog.Properties = properties;

            bool? result = await exitDialog.ShowModalAsync(this);
            if (result != true)
            {
                e.Cancel = true;
            }
        }
    }
}
