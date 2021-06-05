using Mocha.Dialogs;
using Mocha.Dialogs.Extensions;
using Mocha.Dialogs.Extensions.DI;
using Mocha.Dispatching;
using Mocha.Events;
using Mocha.Events.Extensions;
using Mocha.Events.Extensions.DI;
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
        private readonly IDialogFactory _dialogFactory;

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

        public MainWindowViewModel(INavigationService navigationService, IEventService eventService, IDialogFactory dialogFactory)
        {
            _dialogFactory = dialogFactory;

            Navigator = new Navigator(this, navigationService);
            navigationService.NavigationRequested += OnNavigationRequested;
        }

        private async Task OnClosingAsync(AppClosingEventArgs e, IReadOnlyCollection<AsyncEventHandler> invocationList)
        {
            if(invocationList.Where(h => h.Id == "myEvent").ToList().Count > 0)
            {
                return;
            }

            using (IDialogModule<StandardDialogControl> dialog = _dialogFactory.Create<StandardDialogControl>(DialogsIDs.MsgBoxDialog))
            {
                dialog.DataContext.DialogControl.Title = "Application closing";
                dialog.DataContext.DialogControl.Message = "Are you sure you want close the app?";
                dialog.DataContext.DialogControl.PredefinedButtons = "YesNo";
                dialog.DataContext.DialogControl.Icon = "Question";

                bool? result = await dialog.ShowModalAsync();

                if (result != true)
                {
                    e.Cancel = true;
                }
            }
        }

        async void OnLoaded()
        {
            AppEventManager.RequestEventProvider<AppClosingEventArgs>("OnClosingEvent").SubscribeAsync(new AsyncEventHandler<AppClosingEventArgs>(OnClosingAsync, "mainOnClosingHandler"));

            await Navigator.NavigateAsync(NavigationModules.Page1);
            ISettingsSection<GeneralSettings> settings = SettingsManager.Retrieve<GeneralSettings>("myCustomSettings1");

            await settings.UpdateAsync(s =>
            {
                s.MySetting1 = "xxx";
                s.MySetting2 = "yyy";
            });
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
