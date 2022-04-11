using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWinUI.Utils.Xaml
{
    /// <summary>
    /// Allows to retrieve parent of selected element of given type.
    /// <para>Example usage:</para>
    /// <code>
    /// &lt;Button 
    /// local:ParentService.ParentType="Page"
    /// Command="{Binding ParentSource.DataContext.ParentCommand}" 
    /// Content={Binding OriginalSource.OriginalContextText} /&gt;
    /// </code>
    /// </summary>
    public static class ParentService
    {
        public static readonly DependencyProperty ParentTypeProperty =
            DependencyProperty.RegisterAttached(
                "ParentType",
                typeof(Type),
                typeof(ParentService),
                new PropertyMetadata(default(Type), OnParentTypeChanged)
        );

        public static void SetParentType(FrameworkElement element, Type value) => element.SetValue(ParentTypeProperty, value);

        public static Type GetParentType(FrameworkElement element) => (Type)element.GetValue(ParentTypeProperty);

        private static void OnParentTypeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement? hostElement = dependencyObject as FrameworkElement;

            if (hostElement is not null)
            {
                if (hostElement.IsLoaded)
                {
                    SetDataContext(hostElement);
                }
                else
                {
                    hostElement.Loaded += OnTargetLoaded;
                }
            }
        }

        private static void OnTargetLoaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement host = (sender as FrameworkElement)!;
            host.Loaded -= OnTargetLoaded;
            SetDataContext(host);
        }

        private static void SetDataContext(FrameworkElement target)
        {
            Type ancestorType = GetParentType(target);
            if (ancestorType != null)
            {
                object? parent = FindParent(target, ancestorType);
                target.DataContext = new DataContextProxy()
                {
                    OriginalSource = target.DataContext,
                    ParentSource = parent
                };
            }
        }

        private static object? FindParent(DependencyObject dependencyObject, Type parentType)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null)
            {
                return null;
            }

            if (parentType.IsAssignableFrom(parent.GetType()))
            {
                return parent;
            }

            return FindParent(parent, parentType);
        }
    }

    /// <summary>
    /// Provides proxy DataContext for <see cref="ParentService"/> result.
    /// </summary>
    public class DataContextProxy
    {
        /// <summary>
        /// Reference to original DataContext object
        /// </summary>
        public object? OriginalSource { get; set; }

        /// <summary>
        /// Reference to found parent object.
        /// </summary>
        public object? ParentSource { get; set; }
    }
}
