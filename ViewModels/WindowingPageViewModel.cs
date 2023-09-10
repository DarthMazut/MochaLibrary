using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Navigation;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Windows;

namespace ViewModels
{
    public partial class WindowingPageViewModel : ObservableObject, INavigationParticipant
    {
        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        [RelayCommand]
        private async Task OpenWindow()
        {
            using ICustomWindowModule<GenericWindowProperties> testWindowModule
                = WindowManager.RetrieveCustomWindow<GenericWindowProperties>("TestWindow");

            testWindowModule.Properties.Info = "Hello there 😎";
            testWindowModule.StateChanged += (s, e) =>
            {

            };

            object? result = await testWindowModule.OpenAsync();
        }
    }
}
