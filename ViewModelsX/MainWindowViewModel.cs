using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Navigation;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX
{
    public partial class MainWindowViewModel : ObservableObject, IWindowAware
    {
        public IWindowControl WindowControl { get; } = new WindowControl();

        public MainWindowViewModel()
        {
            NavigationServices.MainNavigationService.CurrentModuleChanged += HandleNavigation;
        }

        [ObservableProperty]
        private bool _isFullScreen;

        [ObservableProperty]
        private AppPage? _selectedPage;

        [ObservableProperty]
        private bool _isSettingsInvoked;

        [ObservableProperty]
        private object? _pageContent;

        [ObservableProperty]
        private object? _fullScreenPageContent;

        [RelayCommand]
        private async Task NavigationInvoked()
        {
            IRemoteNavigator remoteNavigator = Navigator.CreateProxy(NavigationServices.MainNavigationServiceId, this);
            if (IsSettingsInvoked)
            {
                await remoteNavigator.NavigateAsync(AppPages.SettingsPage.Id);
            }
            else if (SelectedPage is not null)
            {
                await remoteNavigator.NavigateAsync(SelectedPage.Id);
            }
            else
            {
                throw new Exception("SelectedPage was null while settings item was not invoked. This should not happen!");
            }
            
        }

        private void HandleNavigation(object? sender, CurrentNavigationModuleChangedEventArgs e)
        {
            PageContent = e.CurrentModule.View;
        }
    }
}
