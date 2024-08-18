using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MochaCore.Utils;
using MochaWinUI.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiApplicationX.Pages.Dialogs
{
    public sealed partial class SystemDialogsTab : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(object), typeof(SystemDialogsTab), new PropertyMetadata(null));

        public object? ViewModel
        {
            get { return (object?)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public SystemDialogsTab()
        {
            this.InitializeComponent();
        }

        private void LogsTextChanged(object sender, TextChangedEventArgs e) => ScrollToBottom((TextBox)sender);

        // https://stackoverflow.com/questions/40114620/uwp-c-sharp-scroll-to-the-bottom-of-textbox/41898598#41898598
        private void ScrollToBottom(TextBox textBox)
        {
            var grid = (Grid)VisualTreeHelper.GetChild(textBox, 0);
            for (var i = 0; i <= VisualTreeHelper.GetChildrenCount(grid) - 1; i++)
            {
                object obj = VisualTreeHelper.GetChild(grid, i);
                if (!(obj is ScrollViewer)) continue;
                ((ScrollViewer)obj).ChangeView(0.0f, ((ScrollViewer)obj).ExtentHeight, 1.0f, true);
                break;
            }
        }
    }
}
