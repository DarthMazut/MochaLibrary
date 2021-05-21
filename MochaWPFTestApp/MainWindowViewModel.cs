using Mocha.Dialogs;
using Mocha.Dialogs.Extensions;
using Mocha.Navigation;
using Mocha.Settings;
using MochaWPFTestApp.Settings;
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

        private DelegateCommand _onLoaded;
        public DelegateCommand OnLoadedCommand => _onLoaded ?? (_onLoaded = new DelegateCommand(OnLoaded));

        private DelegateCommand _navigateToPage1Command;
        public DelegateCommand NavigateToPage1Command =>
            _navigateToPage1Command ?? (_navigateToPage1Command = new DelegateCommand(NavigateToPage1));

        private DelegateCommand _navigateToPage2Command;
        public DelegateCommand NavigateToPage2Command =>
            _navigateToPage2Command ?? (_navigateToPage2Command = new DelegateCommand(NavigateToPage2));

        private DelegateCommand _navigateToPage3Command;
        public DelegateCommand NavigateToPage3Command =>
            _navigateToPage3Command ?? (_navigateToPage3Command = new DelegateCommand(NavigateToPage3));

        public MainWindowViewModel()
        {
            Navigator = new Navigator(this, NavigationServices.MainNavigationService);
            NavigationServices.MainNavigationService.NavigationRequested += OnNavigationRequested;
        }

        async void OnLoaded()
        {
            await Navigator.NavigateAsync(NavigationModules.Page1);

            SettingsManager.Retrieve<GeneralSettings>("myCustomSettings1").Update(s => 
            {
                s.MySetting1 = "xxx";
                s.MySetting2 = "yyy";
            });

            IDialogModule<FileDialogControl> dialog;
            //dialog.DataContext.DialogControl
        }

        async void NavigateToPage1()
        {
            await Navigator.NavigateAsync(NavigationModules.Page1);
        }

        async void NavigateToPage2()
        {
            await Navigator.NavigateAsync(NavigationModules.Page2);
        }

        async void NavigateToPage3()
        {
            await Navigator.NavigateAsync(NavigationModules.Page3);
        }

        private void OnNavigationRequested(object sender, NavigationData e)
        {
            Content = e.RequestedModule.View;
        }
    }
}
