using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MochaCore.Dialogs.Extensions;

namespace MochaWinUI.Dialogs
{

    public class StandardMessageDialogModule : ContentDialogModule<StandardMessageDialogProperties>
    {
        // How about ctor with StandardMessageDialogProperties ?

        public StandardMessageDialogModule() : base(new ContentDialog(), null, new StandardMessageDialogProperties()) { }

        /// <inheritdoc/>
        protected override void ApplyProperties(StandardMessageDialogProperties properties, ContentDialog dialog)
        {
            dialog.Title = properties.Title;
            dialog.Content = properties.Message;
            dialog.PrimaryButtonText = properties.ConfirmationButtonText;
            dialog.SecondaryButtonText = properties?.DeclineButtonText;
            dialog.CloseButtonText = properties?.CancelButtonText;
        }
    }

}
