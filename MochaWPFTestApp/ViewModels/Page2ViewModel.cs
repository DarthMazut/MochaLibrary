using Mocha.Dialogs;
using Mocha.Navigation;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using MochaWPFTestApp;
using System.Text;
using System.Threading.Tasks;

namespace MochaWPFTestApp.ViewModels
{
    class Page2ViewModel : INavigatable
    {
        public string Text => "Page 2";

        private DelegateCommand _openDialogCommand;
        public DelegateCommand OpenDialogCommand => _openDialogCommand ?? (_openDialogCommand = new DelegateCommand(OpenDialog));

        private DelegateCommand _navigateCommand;
        public DelegateCommand NavigateCommand => _navigateCommand ?? (_navigateCommand = new DelegateCommand(Navigate));

        private async void Navigate()
        {
            await Navigator.NavigateAsync(NavigationModules.Page1);
        }

        private async void OpenDialog()
        {
            using (IDialogModule myDialog = MochaWPFTestApp.Dialogs.CustomDialog1)
            {
                myDialog.DataContext.DialogControl.Title = "Title";
                //myDialog.DataContext.DialogControl.Message = "Hello World";

                myDialog.DataContext.DialogControl.Opened += (s, e) =>
                {
                    
                };

                myDialog.Closed += (s, e) =>
                {
                    
                };

                myDialog.Disposed += (s, e) =>
                {

                };

                await myDialog.ShowModalAsync();
            }
        }

        public Navigator Navigator { get; }

        public Page2ViewModel(NavigationService navigationService)
        {
            Navigator = new Navigator(this, navigationService);
        }
    }
}
