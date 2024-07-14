using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WinUiApplicationX.Controls
{
    public class NavigationViewEx : NavigationView
    {
        public static readonly DependencyProperty IsSettingsInvokedProperty =
            DependencyProperty.Register(nameof(IsSettingsInvoked), typeof(bool), typeof(NavigationViewEx), new PropertyMetadata(false, IsSettingsInvokedChanged));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(NavigationViewEx), new PropertyMetadata(null));

        public bool IsSettingsInvoked
        {
            get => (bool)GetValue(IsSettingsInvokedProperty);
            set => SetValue(IsSettingsInvokedProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public NavigationViewEx()
        {
            this.ItemInvoked += OnItemInvoked;
        }

        private async void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs e)
        {
            IsSettingsInvoked = e.IsSettingsInvoked;
            await Task.Yield();
            Command?.Execute(default);
        }

        private static void IsSettingsInvokedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NavigationViewEx navigationView)
            {
                navigationView.IsSettingsInvoked = e.NewValue is true;
            }
        }
    }
}
