using Mocha.Dialogs;
using Mocha.Navigation;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private void Navigate()
        {
            Navigator.Navigate(NavigationModules.Page1);
        }

        private async void OpenDialog()
        {
            await Dialogs.OpenDialog.ShowModalAsync();
        }

        public Navigator Navigator { get; }

        public Page2ViewModel()
        {
            Navigator = new Navigator(this, NavigationServices.MainNavigationService);
        }
    }
}
