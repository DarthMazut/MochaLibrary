using Mocha.Dialogs;
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
        public bool? DialogResult { get; set; }
        public object DialogValue { get; set; }
        public DialogParameters DialogParameters { get; set; }
        public DialogActions DialogActions { get; set; }
        public DialogEvents DialogEvents { get; set; }// = new DialogEvents();

        private DelegateCommand _openDialogCommand;
        public DelegateCommand OpenDialogCommand => _openDialogCommand ?? (_openDialogCommand = new DelegateCommand(OpenDialog));

        private DelegateCommand _closeDialogCommand;
        public DelegateCommand CloseDialogCommand => _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand(CloseDialog));

        public MyCustomDialogViewModel()
        {
            //DialogEvents.OnClosing = OnClosing;
        }

        private async void OpenDialog()
        {
            IDialogModule dialog = MochaWPFTestApp.Dialogs.OpenDialog;
            dialog.DataContext.DialogParameters = new DialogParameters()
            {
                Title = "My dialog title",
                InitialDirectory = @"C:\Systemowe",
                Parent = this
            };

            await dialog.ShowModalAsync();
            dialog.Dispose();
        }

        private void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void CloseDialog()
        {
            DialogActions.Close();
        }
    }
}
