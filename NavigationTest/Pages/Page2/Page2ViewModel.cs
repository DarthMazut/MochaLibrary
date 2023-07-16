using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.NavigationEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationTest.Pages.Page2
{
    public partial class Page2ViewModel : ObservableObject, INavigatable
    {
        public INavigator Navigator { get; } = MochaCore.NavigationEx.Navigator.Create();

        [ObservableProperty]
        private string _inputText;

        [RelayCommand]
        private Task NavigateBackward()
        {
            return Navigator.NavigateAsync(navigate => navigate.Back());
        }

        [RelayCommand]
        private Task NavigateForward()
        {
            return Navigator.NavigateAsync(navigate => navigate.To("Page3"));
        }
    }
}
