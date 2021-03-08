using Mocha.Dialogs;
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
    class Page1ViewModel : BindableBase, INavigatable, IOnNavigatingTo, IOnNavigatingFrom, IOnNavigatedTo
    {
        public string Text => "Page 1";

        private string _input;

        private DelegateCommand _navigateCommand;
        public DelegateCommand NavigateCommand => _navigateCommand ?? (_navigateCommand = new DelegateCommand(Navigate));

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

            //Navigator.NavigatingTo += Navigator_NavigatingTo;
            //Navigator.NavigatingFrom += Navigator_NavigatingFrom;
            //Navigator.NavigatedTo += Navigator_NavigatedTo;
        }

        private async void Navigate()
        {
            await Navigator.Navigate(NavigationModules.Page2);
        }

        public async Task OnNavigatingTo(NavigationData navigationData, NavigationCancelEventArgs e)
        {
            if (navigationData.CallingModule.DataContext.GetType() != typeof(MainWindowViewModel))
            {
                IDialogModule dialog = MochaWPFTestApp.Dialogs.MsgBoxDialog;
                dialog.DataContext.DialogParameters = new DialogParameters
                {
                    Title = "Title",
                    Message = "OnNavigatingTo :)",
                    PredefinedButtons = "OK",
                    Icon = "Information"
                };
                await dialog.ShowModalAsync();
                dialog.Dispose();
            }
        }

        public async Task OnNavigatingFrom(NavigationData navigationData, NavigationCancelEventArgs e)
        {
            if (navigationData.CallingModule.DataContext.GetType() != typeof(MainWindowViewModel))
            {
                IDialogModule dialog = MochaWPFTestApp.Dialogs.MsgBoxDialog;
                dialog.DataContext.DialogParameters = new DialogParameters
                {
                    Title = "Title",
                    Message = "OnNavigatingFrom :)",
                    PredefinedButtons = "OK",
                    Icon = "Warning"
                };
                await dialog.ShowModalAsync();
                dialog.Dispose();
            }

            NavigationServices.MainNavigationService.ClearCached(NavigationModules.Page1);
        }

        public async Task OnNavigatedTo(NavigationData navigationData)
        {
            if (navigationData.CallingModule.DataContext.GetType() != typeof(MainWindowViewModel))
            {
                IDialogModule dialog = MochaWPFTestApp.Dialogs.MsgBoxDialog;
                dialog.DataContext.DialogParameters = new DialogParameters
                {
                    Title = "Title",
                    Message = "OnNavigatedTo :)",
                    PredefinedButtons = "OK",
                    Icon = "Error"
                };
                await dialog.ShowModalAsync();
                dialog.Dispose();
            }
        }
    }
}
