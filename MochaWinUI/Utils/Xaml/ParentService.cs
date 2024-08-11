using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.Utils.Xaml
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

        public static readonly DependencyProperty ParentNameProperty =
            DependencyProperty.RegisterAttached(
                "ParentName",
                typeof(string),
                typeof(ParentService),
                new PropertyMetadata(null, OnParentNameChanged));

        public static readonly DependencyProperty ParentProperty =
            DependencyProperty.RegisterAttached(
                "Parent",
                typeof(FrameworkElement),
                typeof(ParentService),
                new PropertyMetadata(null));

        public static void SetParentType(FrameworkElement element, Type? value) => element.SetValue(ParentTypeProperty, value);
        public static Type? GetParentType(FrameworkElement element) => (Type)element.GetValue(ParentTypeProperty);

        public static string? GetParentName(DependencyObject obj) => (string)obj.GetValue(ParentNameProperty);
        public static void SetParentName(DependencyObject obj, string? value) => obj.SetValue(ParentNameProperty, value);

        public static FrameworkElement? GetParent(DependencyObject obj) => (FrameworkElement)obj.GetValue(ParentProperty);
        public static void SetParent(DependencyObject obj, FrameworkElement? value) => obj.SetValue(ParentProperty, value);

        private static void OnParentTypeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement? hostElement = dependencyObject as FrameworkElement;

            if (hostElement is not null && e.NewValue is Type newType)
            {
                if (hostElement.IsLoaded)
                {
                    SetParent(dependencyObject, ParentResolver.FindParentElement(dependencyObject, newType));
                }
                else
                {
                    hostElement.Loaded += OnTargetLoaded;
                }
            }
        }

        private static void OnParentNameChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement? hostElement = dependencyObject as FrameworkElement;

            if (hostElement is not null && e.NewValue is string newName)
            {
                if (hostElement.IsLoaded)
                {
                    SetParent(dependencyObject, ParentResolver.FindParentElement(dependencyObject, newName));
                }
                else
                {
                    hostElement.Loaded += OnTargetLoaded;
                }
            }
        }

        private static void OnTargetLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement host)
            {
                host.Loaded -= OnTargetLoaded;

                if (GetParentType(host) is Type type)
                {
                    SetParent(host, ParentResolver.FindParentElement(host, type));
                }
                else if (GetParentName(host) is string name)
                {
                    SetParent(host, ParentResolver.FindParentElement(host, name));
                }
            }
        }
    }
}
