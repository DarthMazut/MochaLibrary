using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.NavigationEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationTest.Pages.Page1
{
    public partial class Page1ViewModel : ObservableObject, INavigationParticipant
    {
        public string PageName => "My Page 1";

        public INavigator Navigator { get; } = MochaCore.NavigationEx.Navigator.Create();

        [ObservableProperty]
        private string _inputText;

        [RelayCommand]
        private Task NavigateForward()
        {
            return Navigator.NavigateAsync(navigate => navigate.To("Page2"));
        }
    }
}
