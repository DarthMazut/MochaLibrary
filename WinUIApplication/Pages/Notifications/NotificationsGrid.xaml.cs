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

namespace WinUiApplication.Pages.Notifications
{
    public sealed partial class NotificationsGrid : UserControl
    {
        public static readonly DependencyProperty MinColumnWidthProperty =
            DependencyProperty.Register(nameof(MinColumnWidth), typeof(double), typeof(NotificationsGrid), new PropertyMetadata(250d));

        public double MinColumnWidth
        {
            get { return (double)GetValue(MinColumnWidthProperty); }
            set { SetValue(MinColumnWidthProperty, value); }
        }

        public NotificationsGrid()
        {
            this.InitializeComponent();
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            HeaderScrollViewer.ScrollToHorizontalOffset((sender as ScrollViewer)!.HorizontalOffset);
        }
    }
}
