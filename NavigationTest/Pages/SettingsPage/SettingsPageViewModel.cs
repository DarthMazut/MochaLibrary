using MochaCore.NavigationEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationTest.Pages.SettingsPage
{
    public class SettingsPageViewModel : INavigatable
    {
        public INavigator Navigator { get; } = MochaCore.NavigationEx.Navigator.Create();
    }
}
