using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiApplication.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PeoplePage : Page
    {
        public PeoplePage()
        {
            this.InitializeComponent();
            ListBox lb = new();
        }
    }

    public class ParentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DependencyObject dependencyObject && parameter is string type)
            {
                return FindParent(dependencyObject, Type.GetType(type));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private object FindParent(DependencyObject dependencyObject, Type ancestorType)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);
            if (parent == null)
                return null;

            if (ancestorType.IsAssignableFrom(parent.GetType()))
                return parent;

            return FindParent(parent, ancestorType);
        }

    }

    //[MarkupExtensionReturnType(ReturnType = typeof(TextBlock))]
    //public class ParentExtension : MarkupExtension
    //{

    //    public Type OfType { get; set; }

    //    public TextBlock CurrentObject { get; set; }

    //    protected override object ProvideValue(IXamlServiceProvider serviceProvider)
    //    {
    //        var rootObjectProvider = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
    //        var targetObj = rootObjectProvider.TargetObject;
    //        var targetProp = rootObjectProvider.TargetProperty;
    //        return CurrentObject;// FindParent(CurrentObject as DependencyObject, OfType);
    //    }

    //    private object FindParent(DependencyObject dependencyObject, Type ancestorType)
    //    {
    //        DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);
    //        if (parent == null)
    //            return null;

    //        if (ancestorType.IsAssignableFrom(parent.GetType()))
    //            return parent;

    //        return FindParent(parent, ancestorType);
    //    }
    //}
}
