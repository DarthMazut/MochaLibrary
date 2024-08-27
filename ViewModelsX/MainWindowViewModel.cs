using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Dispatching;
using MochaCore.Navigation;
using MochaCore.Notifications;
using MochaCore.Windowing;
using System;
using System.Collections;
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

            NotificationManager.NotificationInteracted += (s, e) =>
            {
                DispatcherManager.GetMainThreadDispatcher().EnqueueOnMainThread(async () =>
                {
                    await _mainNavigator.NavigateAsync(b => b
                        .To(AppPages.NotificationsPage.Id)
                        .WithParameter(e));
                });
            };
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
        private async Task NavigationInvoked()
        {
            NavigationResultData navigationResult = await _mainNavigator.NavigateAsync(SelectedPage!.Id);
            HandleNavigationCancelled(navigationResult);
        }

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

        // When current page rejects navigation request, the item selected on view side
        // has already changed. In order to synchronize we need to select last selected
        // menu page.
        // Other solution (better) would be to subscribe to PreviewItemInvoked, set e.Handled = true
        // and invoke command, but this is impossible since there is no PreviewItemInvoked nor
        // a way to cancel selection change after any menu item has been invoked.
        private void HandleNavigationCancelled(NavigationResultData navigationResult)
        {
            if (navigationResult.Result == NavigationResult.RejectedByCurrent)
            {
                IReadOnlyNavigationStack<INavigationStackItem> navigationHistory = _mainNavigator.Service.NavigationHistory;
                string? previousMenuId = navigationHistory
                    .Select(i => i.Module.Id)
                    .Take(navigationHistory.CurrentIndex + 1)
                    .LastOrDefault(id => AppPages.GetMenuPages()
                        .Any(p => p.Id == id));

                if (previousMenuId is not null)
                {
                    SelectedPage = AppPages.GetById(previousMenuId);
                }
            }
        }
    }
}
