using CommunityToolkit.Mvvm.ComponentModel;
using MochaCore.NavigationEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationTest.Pages.HomePage
{
    public partial class HomePageViewModel : ObservableObject, INavigatable
    {
        public INavigator Navigator { get; } = MochaCore.NavigationEx.Navigator.Create();

        public HomePageViewModel()
        {

        }

        
    }
}
