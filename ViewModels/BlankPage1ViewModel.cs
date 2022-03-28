using MochaCore.Navigation;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BlankPage1ViewModel : INavigatable, IOnNavigatingTo
    {
        public BlankPage1ViewModel()
        {
            Navigator = new Navigator(this, NavigationServices.MainNavigationService);
            NavigateCommand = new DelegateCommand(Navigate);
        }

        public Navigator Navigator { get; }

        public DelegateCommand NavigateCommand { get; }

        private async void Navigate()
        {
            await Navigator.NavigateAsync(NavigationManager.FetchModule(Pages.BlankPage2.Id));
        }

        public void OnNavigatingTo(NavigationData navigationData, NavigationCancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
