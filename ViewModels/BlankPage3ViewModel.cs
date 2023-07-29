using MochaCore.NavigationEx;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BlankPage3ViewModel : INavigationParticipant
    {
        private DelegateCommand? _goBackCommand;

        public INavigator Navigator { get; } = MochaCore.NavigationEx.Navigator.Create();

        public DelegateCommand GoBackCommand => _goBackCommand ??= new DelegateCommand(GoBack);

        private async void GoBack()
        {
            _ = await Navigator.NavigateAsync(Pages.BlankPage1.Id);
        }
    }
}
