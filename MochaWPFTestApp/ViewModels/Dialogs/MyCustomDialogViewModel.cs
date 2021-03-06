using Mocha.Dialogs;
using Prism.Commands;
using System;
using System.Collections.Generic;
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
        public DialogBehaviors DialogBehaviors { get; set; }

        private DelegateCommand _openDialogCommand;
        public DelegateCommand OpenDialogCommand => _openDialogCommand ?? (_openDialogCommand = new DelegateCommand(OpenDialog));

        private DelegateCommand _closeDialogCommand;
        public DelegateCommand CloseDialogCommand => _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand(CloseDialog));

        private async void OpenDialog()
        {
            IDialogModule dialog = MochaWPFTestApp.Dialogs.OpenDialog;
            dialog.DataContext.DialogParameters.Parent = this;
            await dialog.ShowModalAsync();
            dialog.Dispose();
        }

        private void CloseDialog()
        {
            DialogBehaviors.Close?.Invoke();
        }
    }
}
