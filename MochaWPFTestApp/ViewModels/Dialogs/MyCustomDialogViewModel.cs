using Mocha.Dialogs;
using Mocha.Dialogs.Extensions;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWPFTestApp.ViewModels.Dialogs
{
    class MyCustomDialogViewModel : IDialog
    {
        public DialogControl DialogControl { get; }

        private DelegateCommand _openDialogCommand;
        public DelegateCommand OpenDialogCommand => _openDialogCommand ?? (_openDialogCommand = new DelegateCommand(OpenDialog));

        private DelegateCommand _closeDialogCommand;
        public DelegateCommand CloseDialogCommand => _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand(CloseDialog));

        
        public MyCustomDialogViewModel()
        {
            DialogControl = new DialogControl(this);
            DialogControl.Opening += OnOpening;
        }

        private void OnOpening(object sender, EventArgs e)
        {
            using (IDialogModule<StandardDialogControl> msgBoxDialog = MochaWPFTestApp.Dialogs.MsgBoxDialog)
            {
                msgBoxDialog.DataContext.DialogControl.Message = "Dialog just displayed";
                msgBoxDialog.ShowModal();
            }
        }

        private async void OpenDialog()
        {
            IDialogModule<FileDialogControl> dialog = MochaWPFTestApp.Dialogs.OpenDialog;
            dialog.DataContext.DialogControl.Title = "My dialog title";
            dialog.DataContext.DialogControl.InitialDirectory = @"C:\Systemowe";
            dialog.DataContext.DialogControl.Parent = this;

            dialog.DataContext.DialogControl.Closing += (s, e) =>
            {

            };

            await dialog.ShowModalAsync();
            dialog.Dispose();
        }

        private void CloseDialog()
        {
            DialogControl.Close();
        }
    }
}
