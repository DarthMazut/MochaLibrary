using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.NavigationEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationTest.Pages.Page3
{
    public partial class Page3ViewModel : ObservableObject, INavigationParticipant, IOnNavigatedToAsync
    {
        public INavigator Navigator { get; } = MochaCore.NavigationEx.Navigator.Create();

        [ObservableProperty]
        private string _inputText;


        [RelayCommand]
        private Task NavigateBackward()
        {
            return Navigator.NavigateAsync(navigate => navigate.Back());
        }

        public Task OnNavigatedToAsync(OnNavigatedToEventArgs e)
        {
            //return Navigator.NavigateAsync(navigate => navigate.To("Page1"));
            return Task.CompletedTask;
        }
    }
}
