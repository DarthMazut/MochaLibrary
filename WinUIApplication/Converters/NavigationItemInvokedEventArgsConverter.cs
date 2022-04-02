using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using ViewModels.Wrappers;

namespace WinUiApplication.Converters
{
    public class NavigationItemInvokedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is NavigationViewItemInvokedEventArgs args)
            {
                return new NavigationInvokedDetails()
                {
                    InvokedPage = (args.InvokedItemContainer as NavigationViewItem)?.DataContext as ApplicationPage,
                    IsSettingsInvoked = args.IsSettingsInvoked
                };
            }

            throw new ArgumentException($"Provided value must be of type {typeof(NavigationViewItemInvokedEventArgs)}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
