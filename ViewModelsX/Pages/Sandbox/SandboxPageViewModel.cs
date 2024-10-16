using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelsX.Dialogs;

namespace ViewModelsX.Pages.Sandbox
{
    public partial class SandboxPageViewModel : INavigationParticipant
    {
        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        [RelayCommand]
        private Task Test()
        {
            using ICustomDialogModule<StandardMessageDialogProperties> dialogModule = AppDialogs.StandardMessageDialog.Module;
            dialogModule.Properties = new StandardMessageDialogProperties()
            {
                Title = "Test method",
                Message = "Clicked"
            };

            return dialogModule.ShowModalAsync(Navigator.Module.View);
        }
    }
}
