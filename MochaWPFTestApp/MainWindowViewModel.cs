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
    class MainWindowViewModel : BindableBase, INavigatable
    {
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

        void NavigateToPage1()
        {
            Navigator.Navigate(NavigationModules.Page1);
        }

        void NavigateToPage2()
        {
            Navigator.Navigate(NavigationModules.Page2);
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
