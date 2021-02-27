using Mocha.Dialogs;
using Mocha.Navigation;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWPFTestApp
{
    class MainWindowViewModel : BindableBase, INavigatable, IDialog
    {
        public bool? DialogResult { get; set; }
        public object DialogValue { get; set; }
        public DialogParameters Parameters { get; set; }

        private object _content;

        public object Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public Navigator Navigator { get; }

        private DelegateCommand _navigateToPage1Command;
        public DelegateCommand NavigateToPage1Command =>
            _navigateToPage1Command ?? (_navigateToPage1Command = new DelegateCommand(NavigateToPage1));

        private DelegateCommand _navigateToPage2Command;
        public DelegateCommand NavigateToPage2Command =>
            _navigateToPage2Command ?? (_navigateToPage2Command = new DelegateCommand(NavigateToPage2));

        private DelegateCommand _navigateToPage3Command;
        public DelegateCommand NavigateToPage3Command =>
            _navigateToPage3Command ?? (_navigateToPage3Command = new DelegateCommand(NavigateToPage3));

        void NavigateToPage1()
        {
            Navigator.Navigate(NavigationModules.Page1);
        }

        void NavigateToPage2()
        {
            Navigator.Navigate(NavigationModules.Page2);
        }

        void NavigateToPage3()
        {
            Navigator.Navigate(NavigationModules.Page3);
        }

        public MainWindowViewModel()
        {
            Navigator = new Navigator(this, NavigationServices.MainNavigationService);

            NavigationServices.MainNavigationService.NavigationRequested += OnNavigationRequested;

            Navigator.Navigate(NavigationModules.Page1);
        }

        private void OnNavigationRequested(object sender, NavigationData e)
        {
            Content = e.RequestedModule.View;
        }
    }
}
