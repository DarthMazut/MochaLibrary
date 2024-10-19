using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Pages.Navigation
{
    public class NavigationPageViewModel : INavigationParticipant
    {
        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        public string Text => "Navigation";
    }
}
