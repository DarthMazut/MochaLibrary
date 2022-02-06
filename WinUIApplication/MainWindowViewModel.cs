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

        public ICommand OpenDialogCommand => new SimpleCommand(async (o) =>
        {
            //IDialogModule<StandardMessageDialogProperties> messageDialog = DialogManager.GetDialog<StandardMessageDialogProperties>("Dialog1");
            //messageDialog.Properties.CustomProperties.Add("Tick", 5);
            //messageDialog.Properties.Title = "Save changes?";
            //messageDialog.Properties.Message = $"Would you like to save changes befor exit application? ({messageDialog.Properties.CustomProperties["Tick"]})";
            //messageDialog.Properties.ConfirmationButtonText = "Save";
            //messageDialog.Properties.DeclineButtonText = "Don't save";
            //messageDialog.Properties.CancelButtonText = "Cancel";

            //DispatcherTimer timer = new();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += (s, e) =>
            //{
            //    int currentTickValue = (int)messageDialog.Properties.CustomProperties["Tick"];
            //    if (currentTickValue == 0)
            //    {
            //        (messageDialog as IDialogClose).Close();
            //    }
            //    messageDialog.Properties.CustomProperties["Tick"] = --currentTickValue;
            //};
            //timer.Start();

            //await messageDialog.ShowModalAsync(this);
            //timer.Stop();

            IDialogModule<OpenFileDialogProperties> openDialog = DialogManager.GetDialog<OpenFileDialogProperties>("OpenDialog");
            openDialog.Properties.Title = "Dupa!";
            openDialog.Properties.MultipleSelection = true;
            openDialog.Properties.Filters.Add(new ExtensionFilter("Filter1", new List<string> { "txt" }));
            bool? result = await openDialog.ShowModalAsync(this);

            IEventProvider<AppClosingEventArgs> closeEventProvider = AppEventManager.RequestEventProvider<AppClosingEventArgs>("OnClosing");
            closeEventProvider.SubscribeAsync(new AsyncEventHandler<AppClosingEventArgs>(myAsyncOnClosingEvent));

        });

        private async Task myAsyncOnClosingEvent(AppClosingEventArgs e, IReadOnlyCollection<AsyncEventHandler> collection)
        {
            await Task.Delay(5000);
        }
    }
}
