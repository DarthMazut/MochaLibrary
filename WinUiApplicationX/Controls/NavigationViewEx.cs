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
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(NavigationViewEx), new PropertyMetadata(null));

        public static readonly DependencyProperty BackCommandProperty =
            DependencyProperty.Register(nameof(BackCommand), typeof(ICommand), typeof(NavigationViewEx), new PropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public ICommand BackCommand
        {
            get => (ICommand)GetValue(BackCommandProperty);
            set => SetValue(BackCommandProperty, value);
        }

        public NavigationViewEx()
        {
            this.ItemInvoked += OnItemInvoked;
            this.BackRequested += OnBackInvoked;
            this.RegisterPropertyChangedCallback(SelectedItemProperty, OnSelectedItemChanged);
        }

        private void OnSelectedItemChanged(DependencyObject sender, DependencyProperty dp)
        {
            
        }

        private async void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs e)
        {
            await Task.Yield();
            Command?.Execute(default);
        }

        private void OnBackInvoked(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            BackCommand?.Execute(default);
        }
    }
}
