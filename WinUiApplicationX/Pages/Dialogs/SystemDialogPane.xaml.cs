using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ModelX.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiApplicationX.Pages.Dialogs
{
    public sealed partial class SystemDialogPane : UserControl
    {
        //public static readonly DependencyProperty ViewModelProperty =
        //    DependencyProperty.Register(nameof(ViewModel), typeof(object), typeof(SystemDialogPane), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedDialogProperty =
            DependencyProperty.Register(nameof(SelectedDialog), typeof(SystemDialog), typeof(SystemDialogPane), new PropertyMetadata(null));

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(SystemDialogPane), new PropertyMetadata(null));

        public SystemDialog? SelectedDialog
        {
            get => (SystemDialog?)GetValue(SelectedDialogProperty);
            set => SetValue(SelectedDialogProperty, value);
        }

        public ICommand? CloseCommand
        {
            get => (ICommand?)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        //public object? ViewModel
        //{
        //    get => (object?)GetValue(ViewModelProperty);
        //    set => SetValue(ViewModelProperty, value);
        //}

        public SystemDialogPane()
        {
            this.InitializeComponent();
        }
    }
}
