using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Navigation;
using MochaCore.Utils;
using System;
using System.Threading.Tasks;

namespace MochaCoreWinUITestApp.ViewModels
{
    public class Page2ViewModel : INavigatable
    {
        public Page2ViewModel()
        {
            Navigator = new(this, NavigationServices.MainNavigationService);
            OpenDialogCommand = new SimpleCommand(OpenDialog);
            OpenFilePickerCommand = new SimpleCommand(OpenFilePicker);
        }

        public Navigator Navigator { get; }

        public SimpleCommand OpenDialogCommand { get; }

        public SimpleCommand OpenFilePickerCommand { get; }

        private async void OpenDialog(object? obj)
        {
            IDialogModule<ContentDialogControl> testDialog1 = DialogManager.GetDialog<ContentDialogControl>("TestDialog1");
            testDialog1.DataContext.DialogControl.Title = "Hello dialog world!";
            testDialog1.DataContext.DialogControl.PrimaryButtonText = "OK";
            testDialog1.DataContext.DialogControl.SecondaryButtonText = "Not OK";
            testDialog1.DataContext.DialogControl.CloseButtonText = "Clozet";

            testDialog1.Opening += (s, e) =>
            {

            };

            testDialog1.Opened += (s, e) =>
            {

            };

            testDialog1.Closing += (s, e) =>
            {
                e.Cancel = true;
            };

            testDialog1.Closed += (s, e) =>
            {

            };

            testDialog1.Disposed += (s, e) =>
            {

            };

            _ = testDialog1.ShowModalAsync();
            await Task.Delay(5000);
            testDialog1.Close();
            testDialog1.Dispose();
            testDialog1.Close();
        }


        private async void OpenFilePicker(object? obj)
        {
            IDialogModule<FileDialogControl>? saveFileDialog = DialogManager.GetDialog<FileDialogControl>("SaveFileDialog");
            bool? result = await saveFileDialog.ShowModalAsync();
        }
    }
}
