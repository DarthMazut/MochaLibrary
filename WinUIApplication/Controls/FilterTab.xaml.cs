using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MochaCore.Utils;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using ViewModels.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiApplication.Controls
{
    public sealed partial class FilterTab : UserControl
    {
        public FilterTab()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register(nameof(Filter), typeof(PersonFilter), typeof(FilterTab), new PropertyMetadata(null));

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(FilterTabViewModel), typeof(FilterTab), new PropertyMetadata(null));

        public static readonly DependencyProperty FilterAppliedCommandProperty =
            DependencyProperty.Register(nameof(FilterAppliedCommand), typeof(ICommand), typeof(FilterTab), new PropertyMetadata(null));

        public FilterTabViewModel ViewModel
        {
            get { return (FilterTabViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public PersonFilter Filter
        {
            get { return (PersonFilter)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public ICommand FilterAppliedCommand
        {
            get { return (ICommand)GetValue(FilterAppliedCommandProperty); }
            set { SetValue(FilterAppliedCommandProperty, value); }
        }

        private void PropertyNotifier_NotifyRequested(object sender, NotifyDependencyPropertyEventArgs e)
        {
            if (e.DependencyPropertyName == nameof(FilterAppliedCommand))
            {
                FilterAppliedCommand?.Execute(e.Value);
            }
        }
    }
}
