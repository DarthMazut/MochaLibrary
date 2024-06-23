using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX
{
    public partial class MainWindowViewModel : ObservableObject, IWindowAware
    {
        public IWindowControl WindowControl { get; } = new WindowControl();

        [RelayCommand]
        private async Task Click()
        {
            ICustomDialogModule<StandardMessageDialogProperties> stdDialog 
                = DialogManager.GetCustomDialog<StandardMessageDialogProperties>("StdDialog");

            stdDialog.Properties.Title = "Title";
            stdDialog.Properties.Message = "Hello World!";
            stdDialog.Properties.CancelButtonText = "Cancel";

            bool? dialogResult = await stdDialog.ShowModalAsync(WindowControl.View);

            if (dialogResult == true)
            {
                WindowControl.Minimize();
            }
        }
    }
}
