using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationTest
{
    public class AppPagesExtenstion : MarkupExtension
    {
        protected override object ProvideValue(IXamlServiceProvider serviceProvider)
        {
            return AppPages.Collection;
        }
    }
}
