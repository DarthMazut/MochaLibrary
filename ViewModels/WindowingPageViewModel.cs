using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Navigation;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public partial class WindowingPageViewModel : ObservableObject, INavigationParticipant
    {
        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        [RelayCommand]
        private void OpenWindow()
        {
            IWindowModule testWindowModule = WindowManager.RetrieveWindow("TestWindow");
            testWindowModule.Open();
            //testWindowModule.Dispose();
        }
    }
}
