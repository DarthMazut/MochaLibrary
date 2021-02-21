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
    class Page1ViewModel : BindableBase, INavigatable
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

            Navigator.NavigatingTo += Navigator_NavigatingTo;
            Navigator.NavigatingFrom += Navigator_NavigatingFrom;
            Navigator.NavigatedTo += Navigator_NavigatedTo;
        }

        private void Navigator_NavigatedTo(NavigationData navigationData)
        {
            if(navigationData.CallingModule.DataContext.GetType() != typeof(MainWindowViewModel))
            {
                IDialogModule dialog = MochaWPFTestApp.Dialogs.MsgBoxDialog;
                dialog.DataContext.Parameters = new Dictionary<string, object>
                {
                    {DialogParameters.Title, "Title"},
                    {DialogParameters.Caption, "OnNavigatedTo :)"},
                    {DialogParameters.SimpleButtons, "OK"},
                    {DialogParameters.Icon, "Error"}
                };
                dialog.ShowModal();
                dialog.Dispose();
            }
        }

        private void Navigator_NavigatingFrom(NavigationData navigationData, NavigationCancelEventArgs e)
        {
            if (navigationData.CallingModule.DataContext.GetType() != typeof(MainWindowViewModel))
            {
                IDialogModule dialog = MochaWPFTestApp.Dialogs.MsgBoxDialog;
                dialog.DataContext.Parameters = new Dictionary<string, object>
                {
                    {DialogParameters.Title, "Title"},
                    {DialogParameters.Caption, "OnNavigatingFrom :)"},
                    {DialogParameters.SimpleButtons, "OK"},
                    {DialogParameters.Icon, "Warning"}
                };
                dialog.ShowModal();
                dialog.Dispose();
            }

            NavigationServices.MainNavigationService.ClearCached(NavigationModules.Page1);
        }

        private void Navigator_NavigatingTo(NavigationData navigationData, NavigationCancelEventArgs e)
        {
            if (navigationData.CallingModule.DataContext.GetType() != typeof(MainWindowViewModel))
            {
                IDialogModule dialog = MochaWPFTestApp.Dialogs.MsgBoxDialog;
                dialog.DataContext.Parameters = new Dictionary<string, object>
                {
                    {DialogParameters.Title, "Title"},
                    {DialogParameters.Caption, "OnNavigatingTo :)"},
                    {DialogParameters.SimpleButtons, "OK"},
                    {DialogParameters.Icon, "Information"}
                };
                dialog.ShowModal();
                dialog.Dispose();
            }
        }

        private void Navigate()
        {
            Navigator.Navigate(NavigationModules.Page2);
        }
    }
}
