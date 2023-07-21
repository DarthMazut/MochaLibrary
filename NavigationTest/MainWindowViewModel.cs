using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls.Primitives;
using MochaCore.NavigationEx;
using NavigationTest.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationTest
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            INavigationService mainNavigationService = NavigationManager.FetchNavigationService("MainNavigationService");
            mainNavigationService.CurrentModuleChanged += (s, e) =>
            {
                NavigationContent = e.CurrentModule.View;
                SelectedItem = AppPages.GetById(e.CurrentModule.Id);
            };
        }

        [ObservableProperty]
        object? _navigationContent;

        [ObservableProperty]
        AppPage? _selectedItem;

        [RelayCommand]
        private Task NavigationItemInvoked(NavigationInvokedDetails e)
        {
            IRemoteNavigator navigator = Navigator.CreateProxy("MainNavigationService", this);

            if (e.InvokedPage is not null)
            {
                return navigator.NavigateAsync(navigate => navigate.To(e.InvokedPage.Id));
            }
            else
            {
                return navigator.NavigateAsync(AppPages.SettingsPage.Id);
            }
        }
    }
}
