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
        private readonly IRemoteNavigator _mainNavigator;

        public IWindowControl WindowControl { get; } = new WindowControl();

        public MainWindowViewModel()
        {
            _mainNavigator = Navigator.CreateProxy(NavigationServices.MainNavigationServiceId, this);

            NavigationServices.MainNavigationService.CurrentModuleChanged += HandleNavigation;
            NavigationServices.MainNavigationService.Initialize();
        }

        [ObservableProperty]
        private bool _canGoBack;

        [ObservableProperty]
        private bool _isFullScreen;

        [ObservableProperty]
        private AppPage? _selectedPage;

        [ObservableProperty]
        private object? _pageContent;

        [ObservableProperty]
        private object? _fullScreenPageContent;

        [RelayCommand]
        private Task NavigationInvoked() => _mainNavigator.NavigateAsync(SelectedPage!.Id);

        [RelayCommand]
        private Task NavigationBack() => _mainNavigator.NavigateBackAsync();

        private void HandleNavigation(object? sender, CurrentNavigationModuleChangedEventArgs e)
        {
            CanGoBack = _mainNavigator.CanNavigateBack;
            AppPage currentPage = AppPages.GetById(e.CurrentModule.Id);
            SelectedPage = currentPage;
            IsFullScreen = currentPage.IsFullScreen;
            PageContent = currentPage.IsFullScreen ? null : e.CurrentModule.View;
            FullScreenPageContent = currentPage.IsFullScreen ? e.CurrentModule.View : null;
        }
    }
}
