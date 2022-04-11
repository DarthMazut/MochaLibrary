using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWinUI.Utils.Xaml
{
    /// <summary>
    /// Provides a root element of current XAML.
    /// <para>Example usage:</para>
    /// <code>
    /// &lt;Button Command="{Binding DataContext.MyCommand, Source={xamlUtils:RootElement}}"/&gt;
    /// </code>
    /// </summary>
    [MarkupExtensionReturnType(ReturnType = typeof(FrameworkElement))]
    public class RootElementExtension : MarkupExtension
    {
        protected override object? ProvideValue(IXamlServiceProvider serviceProvider)
        {
            IRootObjectProvider? service = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            return service?.RootObject ?? null;
        }
    }
}
