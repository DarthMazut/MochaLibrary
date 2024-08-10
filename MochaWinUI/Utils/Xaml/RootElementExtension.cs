using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.Utils.Xaml
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

    public class ParentElementExtension : MarkupExtension
    {
        public string? Name { get; set; }

        public Type? Type { get; set; }

        protected override object ProvideValue(IXamlServiceProvider serviceProvider)
        {
            IProvideValueTarget? provideTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            object? targetObject = provideTarget?.TargetObject;
            
            if (Name is not null)
            {
                return ParentResolver.FindParentElement(targetObject, Name);
            }

            if (Type is not null)
            {
                return ParentResolver.FindParentElement(targetObject, Type);
            }

            return null;
        }
    }
}
