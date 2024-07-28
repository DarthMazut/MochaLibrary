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
    public class FlyoutEx
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(FlyoutEx), new PropertyMetadata(0, CommandChanged));

        public static ICommand GetCommand(DependencyObject obj) => (ICommand)obj.GetValue(CommandProperty);

        public static void SetCommand(DependencyObject obj, ICommand value) => obj.SetValue(CommandProperty, value);

        // <Button Flyout.Command="" />
        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Button targetButton)
            {
                targetButton.Command = new CommandWrapper(targetButton, (ICommand)e.NewValue);
            }
        }

        private class CommandWrapper(FrameworkElement commandHost, ICommand coreCommand) : ICommand
        {
            public event EventHandler? CanExecuteChanged
            {
                add => coreCommand.CanExecuteChanged += value;
                remove => coreCommand.CanExecuteChanged -= value;
            }

            public bool CanExecute(object? parameter) => coreCommand.CanExecute(parameter);

            public void Execute(object? parameter)
            {
                CloseNearestFlyout(commandHost);
                //coreCommand.Execute(parameter);
            }

            private void CloseNearestFlyout(FrameworkElement commandHost)
            {
                FrameworkElement? parent = commandHost;
                do
                {
                    parent = parent.Parent as FrameworkElement;//VisualTreeHelper.GetParent(parent);
                    if (parent is Popup flyout)
                    {
                        flyout.IsOpen = false;
                        return;
                    }
                }
                while (parent != null);
            }
        }
    }
}
