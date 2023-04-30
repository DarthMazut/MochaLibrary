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
    public partial class MainWindowViewModel : ObservableObject, INavigatable
    {
        public MainWindowViewModel()
        {
            Navigator.Initialized += (s, e) => Navigator.Service.CurrentModuleChanged += (s, e) =>
            {
                NavigationContent = e.CurrentModule.View;
                SelectedItem = AppPages.GetById(e.CurrentModule.Id);
            };
        }

        public Navigator Navigator { get; } = new();

        [ObservableProperty]
        object _navigationContent;

        [ObservableProperty]
        AppPage? _selectedItem;

        [RelayCommand]
        private Task NavigationItemInvoked(NavigationInvokedDetails e)
        {
            if (e.InvokedPage is not null)
            {
                return Navigator.NavigateAsync(navigate => navigate.To(e.InvokedPage.Id));
            }
            else
            {
                return Navigator.NavigateAsync(AppPages.SettingsPage.Id);
            }
        }
    }
}
