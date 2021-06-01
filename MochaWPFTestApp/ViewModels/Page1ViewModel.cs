using Mocha.Behaviours;
using Mocha.Dialogs;
using Mocha.Dialogs.Extensions;
using Mocha.Dialogs.Extensions.DI;
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
    class Page1ViewModel : BindableBase, INavigatable, IOnNavigatingToAsync, IOnNavigatingFromAsync, IOnNavigatedToAsync, IOnNavigatingFrom
    {
        private readonly IBehaviourService _behaviourService;

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

        public Page1ViewModel(NavigationService navigationService, IBehaviourService behaviourService)
        {
            _behaviourService = behaviourService;
            Navigator = new Navigator(this, navigationService);
        }

        private async void Navigate()
        {
            await Navigator.NavigateAsync(NavigationModules.Page2);
            var result = await _behaviourService.Recall<object, Task<string>>("exit").Execute(null);
        }

        public async Task OnNavigatingToAsync(NavigationData navigationData, NavigationCancelEventArgs e)
        {
            if (navigationData.CallingModule.DataContext.GetType() != typeof(MainWindowViewModel))
            {
                IDialogModule<StandardDialogControl> dialog = MochaWPFTestApp.Dialogs.MsgBoxDialog;
                dialog.DataContext.DialogControl.Title = "Title";
                dialog.DataContext.DialogControl.Message = "OnNavigatingTo :)";
                dialog.DataContext.DialogControl.PredefinedButtons = "OK";
                dialog.DataContext.DialogControl.Icon = "Information";

                await dialog.ShowModalAsync();
                dialog.Dispose();
            }
        }

        public async Task OnNavigatingFromAsync(NavigationData navigationData, NavigationCancelEventArgs e)
        {
            if (navigationData.CallingModule.DataContext.GetType() != typeof(MainWindowViewModel))
            {
                IDialogModule<StandardDialogControl> dialog = MochaWPFTestApp.Dialogs.MsgBoxDialog;
                dialog.DataContext.DialogControl.Title = "Title";
                dialog.DataContext.DialogControl.Message = "OnNavigatingFrom :)";
                dialog.DataContext.DialogControl.PredefinedButtons = "OK";
                dialog.DataContext.DialogControl.Icon = "Warning";

                await dialog.ShowModalAsync();
                dialog.Dispose();
            }
        }

        public async Task OnNavigatedToAsync(NavigationData navigationData)
        {
            if (navigationData.CallingModule.DataContext.GetType() != typeof(MainWindowViewModel))
            {
                IDialogModule<StandardDialogControl> dialog = new DialogFactory().Create<StandardDialogControl>(DialogsIDs.MsgBoxDialog); // MochaWPFTestApp.Dialogs.MsgBoxDialog;
                dialog.DataContext.DialogControl.Title = "Title";
                dialog.DataContext.DialogControl.Message = "OnNavigatedTo :)";
                dialog.DataContext.DialogControl.PredefinedButtons = "OK";
                dialog.DataContext.DialogControl.Icon = "Error";

                await dialog.ShowModalAsync();
                dialog.Dispose();
            }
        }

        public void OnNavigatingFrom(NavigationData navigationData, NavigationCancelEventArgs e)
        {
            // this fires before async
        }
    }
}
