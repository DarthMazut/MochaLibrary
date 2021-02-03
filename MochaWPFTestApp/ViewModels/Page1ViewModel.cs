using Mocha.Navigation;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWPFTestApp.ViewModels
{
    class Page1ViewModel : BindableBase, INavigatable
    {
        public string Text => "Page 1";

        private string _input;

        private DelegateCommand _navigateCommand;
        public DelegateCommand NavigateCommand => _navigateCommand ?? (_navigateCommand = new DelegateCommand(Navigate));

        private void Navigate()
        {
            Navigator.Navigate(NavigationModules.Page2);
        }

        public string Input
        {
            get => _input;
            set => SetProperty(ref _input, value);
        }

        public Navigator Navigator { get; }

        public Page1ViewModel()
        {
            Navigator = new Navigator(this, NavigationServices.MainNavigationService);
            Navigator.SaveCurrent = true;
        }
    }
}
