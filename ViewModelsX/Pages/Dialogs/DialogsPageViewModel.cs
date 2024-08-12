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
        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        public DialogsPageViewModel()
        {
            SystemDialogsViewModel = new(this);
        }

        public SystemDialogsTabViewModel SystemDialogsViewModel { get; }
    }
}
