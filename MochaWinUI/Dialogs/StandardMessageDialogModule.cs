using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MochaCore.Dialogs.Extensions;

namespace MochaWinUI.Dialogs
{
    /*
    public class StandardMessageDialogModule : ContentDialogModule<StandardMessageDialogProperties>
    {
        public StandardMessageDialogModule(Window mainWindow) : base(mainWindow, new ContentDialog(), null, new StandardMessageDialogProperties()) { }

        // How about ctor with StandardMessageDialogProperties ?

        protected override void ApplyPropertiesCore(StandardMessageDialogProperties? properties, ContentDialog view)
        {
            view.Title = properties.Title;
            view.Content = properties.Message;
            view.PrimaryButtonText = properties.ConfirmationButtonText;
            view.SecondaryButtonText = properties?.DeclineButtonText;
            view.CloseButtonText = properties?.CancelButtonText;
        }
    }
    */
}
