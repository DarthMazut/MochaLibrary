using MochaCore.Navigation;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BlankPage3ViewModel : INavigatable
    {
        private DelegateCommand _goBackCommand;

        public BlankPage3ViewModel()
        {
            Navigator = new(this, NavigationServices.MainNavigationService);
        }

        public Navigator Navigator { get; }

        public DelegateCommand GoBackCommand => _goBackCommand ??= new DelegateCommand(GoBack);

        private async void GoBack()
        {
            _ = await Navigator.NavigateAsync(Pages.BlankPage1.GetNavigationModule());
        }
    }
}
