using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using MochaCore.Dialogs;
using MochaCore.Navigation;
using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MochaWinUI.Utils
{
    /// <summary>
    /// Provides default algorithms for searching the parents of given elements.
    /// </summary>
    public static class ParentResolver
    {
        /// <summary>
        /// Searches for <see cref="XamlRoot"/> of given object. 
        /// Returns <see langword="null"/> if no such could be found.
        /// </summary>
        /// <param name="host">Object whose parent <see cref="XamlRoot"/> is to be found.</param>
        public static XamlRoot? FindParentXamlRoot(object? host)
        {
            if (host is UIElement uiElement)
            {
                return uiElement.XamlRoot;
            }

            if (host is Window window)
            {
                return window.Content.XamlRoot;
            }

            return null;
        }

        /// <summary>
        /// Searches for parent <see cref="Window"/> of provided element.
        /// Returns <see langword="null"/> if no such could be found.
        /// </summary>
        /// <param name="element">Object whose parent is to be found.</param>
        public static Window? FindParentWindow(object? element)
        {
            if (element is Window window)
            {
                return window;
            }

            if (element is FrameworkElement frameworkElement)
            {
                return FindWindowByXamlRoot(frameworkElement) ?? FindWindowByTraverseUp(frameworkElement);
            }

            return null;
        }

        private static Window? FindWindowByTraverseUp(FrameworkElement frameworkElement)
        {
            List<UIElement> rootContents = new();

            foreach (IBaseWindowModule windowModule in WindowManager.GetOpenedModules())
            {
                if (windowModule.View is Window window)
                {
                    rootContents.Add(window.Content);
                    //TraverseVisualTree(frameworkElement, fe => window.Content == fe);
                }
            }

            Window? currentWindow = null;
            if (rootContents.Any())
            {
                _ = TraverseVisualTree(frameworkElement, fe =>
                {
                    currentWindow
                    return rootContents.Any(rc => rc == fe);
                });
            }

            return null;
        }

        private static Window? FindWindowByXamlRoot(UIElement uiElement)
        {
            foreach (IBaseWindowModule windowModule in WindowManager.GetOpenedModules())
            {
                if (windowModule.View is Window window)
                {
                    if (window.Content == uiElement)
                    {
                        return window;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Searches for a first parent element with the specified name.
        /// Returns <see langword="null"/> if no such could be found.
        /// </summary>
        /// <param name="host">The element whose parent is to be found.</param>
        /// <param name="elementName">The name of the parent element to be found.</param>
        public static FrameworkElement? FindParentElement(object? host, string elementName)
            => TraverseVisualTree(host, e => e.Name.Equals(elementName));

        /// <summary>
        /// Searches for a first parent element of the specified type.
        /// Returns <see langword="null"/> if no such could be found.
        /// </summary>
        /// <param name="host">The element whose parent is to be found.</param>
        /// <param name="elementType">The type of the parent element to be found.</param>
        public static FrameworkElement? FindParentElement(object? host, Type elementType)
            => TraverseVisualTree(host, e => e.GetType().Equals(elementType));

        private static FrameworkElement? TraverseVisualTree(object? host, Predicate<FrameworkElement> predicate)
        {
            if (host is FrameworkElement currentElement)
            {
                while (VisualTreeHelper.GetParent(currentElement) is FrameworkElement parentElement)
                {
                    currentElement = parentElement;
                    if (predicate.Invoke(currentElement))
                    {
                        return currentElement;
                    }
                }
            }

            return null;
        }
    }
}
