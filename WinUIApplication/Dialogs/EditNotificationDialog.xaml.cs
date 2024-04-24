using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiApplication.Dialogs
{
    public sealed partial class EditNotificationDialog : ContentDialog
    {
        public EditNotificationDialog()
        {
            this.InitializeComponent();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SetDialogMargin();
        }

        private void SetDialogMargin() => (GetTemplateChild("BackgroundElement") as Border)!.Margin = new Thickness(20);
    }
}
