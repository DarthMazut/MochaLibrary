using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Notifications;

namespace WinUiApplication.Pages.Notifications
{
    public class NotificationSchemaTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? StandardSchemaTemplate { get; set; }
        public DataTemplate? HeroImageSchemaTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
            => item switch
            {
                NotificationStandardSchema => StandardSchemaTemplate ?? new(),
                NotificationHeroImageSchema => HeroImageSchemaTemplate ?? new(),
                _ => throw new NotImplementedException(),
            };
    }
}
