using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplicationX.Utils.Xaml
{
    public static class IsPointerOverService
    {
        public static readonly DependencyProperty ObservePointerOverProperty =
            DependencyProperty.RegisterAttached(
                "ObservePointerOver",
                typeof(bool),
                typeof(IsPointerOverService),
                new PropertyMetadata(false, OnObservePointerOverChanged));

        public static readonly DependencyProperty IsPointerOverProperty =
           DependencyProperty.RegisterAttached(
               "IsPointerOver",
               typeof(bool),
               typeof(IsPointerOverService),
               new PropertyMetadata(false));

        public static bool GetIsPointerOver(DependencyObject obj) => (bool)obj.GetValue(IsPointerOverProperty);

        public static void SetIsPointerOver(DependencyObject obj, bool value) => obj.SetValue(IsPointerOverProperty, value);

        public static bool GetObservePointerOver(DependencyObject obj) => (bool)obj.GetValue(ObservePointerOverProperty);

        public static void SetObservePointerOver(DependencyObject obj, bool value) => obj.SetValue(ObservePointerOverProperty, value);

        private static void OnObservePointerOverChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement frameworkElement)
            {
                if (e.NewValue is true)
                {
                    frameworkElement.PointerEntered += ElementPointerEntered;
                    frameworkElement.PointerExited += ElementPointerExited;
                }
                else
                {
                    frameworkElement.PointerEntered -= ElementPointerEntered;
                    frameworkElement.PointerExited -= ElementPointerExited;
                }
            }
        }

        private static void ElementPointerEntered(object sender, PointerRoutedEventArgs e)
            => (sender as DependencyObject)?.SetValue(IsPointerOverProperty, true);

        private static void ElementPointerExited(object sender, PointerRoutedEventArgs e)
            => (sender as DependencyObject)?.SetValue(IsPointerOverProperty, false);
    }
}
