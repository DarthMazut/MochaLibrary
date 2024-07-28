using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WinUiApplicationX.Utils.Xaml
{
    public class FlyoutExtensions
    {
        public static readonly DependencyProperty CloseOnInvokeProperty =
            DependencyProperty.RegisterAttached("CloseOnInvoke", typeof(bool), typeof(FlyoutExtensions), new PropertyMetadata(false, PropertyChanged));

        public static bool GetCloseOnInvoke(DependencyObject obj) => (bool)obj.GetValue(CloseOnInvokeProperty);

        public static void SetCloseOnInvoke(DependencyObject obj, bool value) => obj.SetValue(CloseOnInvokeProperty, value);

        private static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Button targetButton && e.NewValue is true)
            {
                targetButton.Click += (s, e) =>
                {
                    CloseNearestFlyout(targetButton);
                };
            }
        }

        private static void CloseNearestFlyout(FrameworkElement target)
        {
            FrameworkElement? parent = target;
            while (parent is not null)
            {
                if (parent is Popup popup)
                {
                    popup.IsOpen = false;
                    return;
                }

                parent = parent.Parent as FrameworkElement;
            }
        }
    }
}
