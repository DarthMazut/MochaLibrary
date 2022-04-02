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
    public class BlankPage1ViewModel : BindableBase, INavigatable
    {
        private bool _isActive;
        private CancellationTokenSource _cts = new();

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
    }
}
