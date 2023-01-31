using Microsoft.UI.Xaml;
using MochaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRT;

namespace MochaWinUI.Utils
{
    public class DependencyPropertyNotifier : FrameworkElement
    {
        public event EventHandler<NotifyDependencyPropertyEventArgs>? NotifyRequested;

        public static readonly DependencyProperty BindingProperty =
            DependencyProperty.Register(
                nameof(Binding), 
                typeof(ControlNotifierValue), 
                typeof(DependencyPropertyNotifier), 
                new PropertyMetadata(null, BindingPropertyChangedCallback));

        private static void BindingPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DependencyPropertyNotifier self)
            {
                self.NotifyRequested?.Invoke(self, NotifyDependencyPropertyEventArgs.FromControlNotifierValue((ControlNotifierValue)e.NewValue));
            }
        }

        public ControlNotifierValue Binding
        {
            get { return (ControlNotifierValue)GetValue(BindingProperty); }
            set { SetValue(BindingProperty, value); }
        }
    }
}
