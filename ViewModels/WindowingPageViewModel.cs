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
            IWindowModule mainWindowModule = WindowManager.GetOpenedModules().FirstOrDefault();

            IWindowModule testWindowModule = WindowManager.RetrieveWindow("TestWindow");
            if (testWindowModule is ICustomWindowModule<GenericWindowProperties> module)
            {
                
            }

            await testWindowModule.OpenAsync(mainWindowModule);
            testWindowModule.Dispose();
        }
    }
}
