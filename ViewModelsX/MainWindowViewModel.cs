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
            //ICustomDialogModule<MyDialogProperties> customDialogModule
            //    = DialogManager.GetCustomDialog<MyDialogProperties>("CustomDialog");

            //bool? result = await customDialogModule.ShowModalAsync(WindowControl.View);

            //ICustomDialogModule<StandardMessageDialogProperties> stdDialogModule =
            //    DialogManager.GetCustomDialog<StandardMessageDialogProperties>("StdDialog");

            //stdDialogModule.Properties = new StandardMessageDialogProperties()
            //{
            //    ConfirmationButtonText = "Confirm",
            //    CancelButtonText = "Cancel",
            //    DeclineButtonText = "Decline",
            //    Title = "Msg title",
            //    Message = "Hello there?",
            //    Icon = StandardMessageDialogIcons.Error
            //};

            //bool? dialogResult = await stdDialogModule.ShowModalAsync(WindowControl.View);

            IDialogModule<OpenFileDialogProperties> openFileDialogModule
                = DialogManager.GetDialog<OpenFileDialogProperties>("OpenDialog");

            openFileDialogModule.Properties.Title = "My dialog title xD";
            openFileDialogModule.Properties.TrySetInitialDirectory(Environment.SpecialFolder.Desktop);
            openFileDialogModule.Properties.Filters = new List<ExtensionFilter>()
            {
                new ExtensionFilter("Image", "jpg"),
                new ExtensionFilter("Executable", "exe")
            };

            bool? result = await openFileDialogModule.ShowModalAsync(WindowControl.View);
        }
    }
}
