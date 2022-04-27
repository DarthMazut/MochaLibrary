using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplication.Controls
{
    
    public class ValidationTextBox : TextBox
    {
        public Func<string, bool> ValidationFunction
        {
            get { return (Func<string, bool>)GetValue(ValidationFunctionProperty); }
            set { SetValue(ValidationFunctionProperty, value); }
        }

        public static readonly DependencyProperty ValidationFunctionProperty =
            DependencyProperty.Register(nameof(ValidationFunction), typeof(Func<string, bool>), typeof(ValidationTextBox), new PropertyMetadata(null));


        public ValidationTextBox()
        {
            TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            bool? isValid = ValidationFunction?.Invoke(((TextBox)sender).Text);

            if (isValid == false)
            {
                this.Resources["TextControlBorderBrush"] = new SolidColorBrush(Colors.Red);
            }
            else
            {
                this.Resources["TextControlBorderBrush"] = Application.Current.Resources["SystemControlForegroundBaseMediumBrush"];
            }

            
        }
    }
}
