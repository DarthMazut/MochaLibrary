using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ViewModelsX.Pages.Notfifications;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiApplicationX.Pages.Notifications
{
    public sealed partial class NotificationsGeneralTab : UserControl
    {

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(NotificationsGeneralTabViewModel), typeof(NotificationsGeneralTab), new PropertyMetadata(null));

        public NotificationsGeneralTabViewModel ViewModel
        {
            get => (NotificationsGeneralTabViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public NotificationsGeneralTab()
        {
            this.InitializeComponent();
        }
    }
}
