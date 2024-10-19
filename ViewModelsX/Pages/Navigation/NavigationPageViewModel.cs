using CommunityToolkit.Mvvm.ComponentModel;
using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Pages.Navigation
{
    public partial class NavigationPageViewModel : ObservableObject, INavigationParticipant, IOnNavigatedTo, IOnNavigatedFrom
    {
        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        public void OnNavigatedTo(OnNavigatedToEventArgs e)
        {
            NavigationServices.InternalNavigationService.CurrentModuleChanged += InternalNavigationRequested;
            NavigationServices.InternalNavigationService.Initialize();
        }

        public void OnNavigatedFrom(OnNavigatedFromEventArgs e)
        {
            NavigationServices.InternalNavigationService.CurrentModuleChanged -= InternalNavigationRequested;
            NavigationServices.InternalNavigationService.Uninitialize();
        }

        public string Text => "Navigation";

        [ObservableProperty]
        private object? _internalNavigationContent;

        private void InternalNavigationRequested(object? sender, CurrentNavigationModuleChangedEventArgs e)
        {
            InternalNavigationContent = e.CurrentModule.View;
        }
    }
}
