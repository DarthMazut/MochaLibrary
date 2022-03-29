using MochaCore.Navigation;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class BlankPage1ViewModel : BindableBase, INavigatable, IOnNavigatingTo, IOnNavigatedToAsync
    {
        private bool _isActive;

        public BlankPage1ViewModel()
        {
            Navigator = new Navigator(this, NavigationServices.MainNavigationService);
            NavigateCommand = new DelegateCommand(Navigate);
        }

        public Navigator Navigator { get; }

        public bool IsActive 
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public DelegateCommand NavigateCommand { get; }

        private async void Navigate()
        {
            await Navigator.NavigateAsync(NavigationManager.FetchModule(Pages.BlankPage2.Id));
        }

        public void OnNavigatingTo(NavigationData navigationData, NavigationCancelEventArgs e)
        {
            //e.Cancel = true;
        }

        public async Task OnNavigatedToAsync(NavigationData navigationData)
        {
            IsActive = true;
            await Task.Delay(5000);
            IsActive = false;
            await Navigator.NavigateAsync(Pages.BlankPage3.Module);
        }
    }
}
