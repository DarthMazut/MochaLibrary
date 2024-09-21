using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Navigation;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using ViewModelsX.Windows;

namespace ViewModelsX.Pages.Windowing
{
    public partial class WindowingPageViewModel : ObservableObject, INavigationParticipant
    {
        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        [ObservableProperty]
        private string _test = "Hello Windowing Page ;)";

        [RelayCommand]
        private async Task OpenWindow()
        {
            IWindowModule<WindowingGeneralWindowProperties> windowModule = AppWindows.WindowingGeneralWindow.Module;

            await windowModule.OpenAsync();

            Test = "Closed :)";
        }
    }
}
