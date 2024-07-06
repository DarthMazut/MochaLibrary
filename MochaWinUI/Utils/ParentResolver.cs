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
    /// Provides a default algorithms for searching parents of given element.
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
        /// Searches for parent <see cref="Window"/> of provided object.
        /// Returns <see langword="null"/> if no such could be found.
        /// </summary>
        /// <param name="host">Object whose parent is to be found.</param>
        public static Window? FindParentWindow(object? host)
        {
            if (host is UIElement uiElement)
            {
                return FindWindowWithContent(FindTopElement(uiElement));
            }

            if (host is Window window)
            {
                return window;
            }

            return null;
        }

        private static Window? FindWindowWithContent(UIElement? topElement)
        {
            if (topElement is null)
            {
                return null;
            }

            foreach (IBaseWindowModule windowModule in WindowManager.GetOpenedModules())
            {
                if (windowModule.View is Window window)
                {
                    if (window.Content == topElement)
                    {
                        return window;
                    }
                }
            }

            return null;
        }

        private static FrameworkElement? FindTopElement(object? root)
        {
            if (root is FrameworkElement currentElement)
            {
                while (VisualTreeHelper.GetParent(currentElement) is FrameworkElement parentElement)
                {
                    currentElement = parentElement;
                }

                return currentElement;
            }

            return null;
        }
    }
}
