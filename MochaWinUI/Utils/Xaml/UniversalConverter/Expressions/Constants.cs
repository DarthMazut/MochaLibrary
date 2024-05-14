using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.Utils.Xaml.UniversalConverter
{
    public class Visible : MarkupExtension
    {
        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => Visibility.Visible;
    }

    public class Collapsed : MarkupExtension
    {
        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => Visibility.Collapsed;
    }

    public class True : MarkupExtension
    {
        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => true;
    }

    public class False : MarkupExtension
    {
        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => false;
    }

    public class EmptyString : MarkupExtension
    {
        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => string.Empty;
    }
}
