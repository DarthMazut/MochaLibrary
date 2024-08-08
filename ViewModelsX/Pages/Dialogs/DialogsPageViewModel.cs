using CommunityToolkit.Mvvm.ComponentModel;
using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Pages.Dialogs
{
    public class DialogsPageViewModel : ObservableObject, INavigationParticipant
    {
        public INavigator Navigator => MochaCore.Navigation.Navigator.Create();

        public SystemDialogsTabViewModel SystemDialogsViewModel { get; } = new();
    }
}
