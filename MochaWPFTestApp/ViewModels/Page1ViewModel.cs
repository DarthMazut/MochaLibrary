using Mocha.Behaviours;
using Mocha.Behaviours.Extensions.DI;
using Mocha.Dialogs;
using Mocha.Dialogs.Extensions;
using Mocha.Dialogs.Extensions.DI;
using Mocha.Events;
using Mocha.Events.Extensions;
using Mocha.Events.Extensions.DI;
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
        private readonly IDialogFactory _dialogFactory;
        private readonly IEventService _eventService;

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

        public Page1ViewModel(INavigationService navigationService, IBehaviourService behaviourService, IDialogFactory dialogFactory, IEventService eventService)
        {
            _behaviourService = behaviourService;
            _dialogFactory = dialogFactory;
            _eventService = eventService;

            Navigator = new Navigator(this, navigationService);
            Navigator.SaveCurrent = true;
        }

        private async Task OnAppClosing(AppClosingEventArgs e, IReadOnlyCollection<AsyncEventHandler> invocationList)
        {
            using (var dialog = _dialogFactory.Create<StandardDialogControl>(DialogsIDs.MsgBoxDialog))
            {
                await dialog.ShowModalAsync();
                e.Cancel = true;
            }
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
                //IDialogModule<StandardDialogControl> dialog = MochaWPFTestApp.Dialogs.MsgBoxDialog;
                IDialogModule<StandardDialogControl> dialog = _dialogFactory.Create<StandardDialogControl>(DialogsIDs.MsgBoxDialog);
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

            _eventService.RequestEventProvider<AppClosingEventArgs>("OnClosingEvent").UnsubscribeAsync(OnAppClosing);
        }

        public async Task OnNavigatedToAsync(NavigationData navigationData)
        {
            _eventService.RequestEventProvider<AppClosingEventArgs>("OnClosingEvent").SubscribeAsync(new AsyncEventHandler<AppClosingEventArgs>(OnAppClosing, "myEvent", -1));

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
