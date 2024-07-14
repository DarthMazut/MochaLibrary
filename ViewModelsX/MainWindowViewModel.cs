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
            NavigationServices.MainNavigationService.Initialize();
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
            string? targetId = IsSettingsInvoked ? AppPages.SettingsPage.Id : SelectedPage?.Id;
            await Navigator.CreateProxy(NavigationServices.MainNavigationServiceId,this)
                .NavigateAsync(
                    targetId ??
                    throw new Exception("SelectedPage was null while settings item was not invoked. This should not happen!"));
        }

        private void HandleNavigation(object? sender, CurrentNavigationModuleChangedEventArgs e)
        {
            AppPage currentPage = AppPages.GetById(e.CurrentModule.Id);
            IsSettingsInvoked = currentPage == AppPages.SettingsPage ? true : false;
            SelectedPage = currentPage.IsMenuPage ? currentPage : SelectedPage;
            IsFullScreen = currentPage.IsFullScreen;
            PageContent = currentPage.IsFullScreen ? null : e.CurrentModule.View;
            FullScreenPageContent = currentPage.IsFullScreen ? e.CurrentModule.View : null;
        }
    }
}
