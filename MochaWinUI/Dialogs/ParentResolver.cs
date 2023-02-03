using Microsoft.UI.Xaml;
using MochaCore.Dialogs;
using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWinUI.Dialogs
{
    /// <summary>
    /// Provides a default algorithms for searching parent <see cref="XamlRoot"/> 
    /// and parent <see cref="Window"/> for given object.
    /// </summary>
    public static class ParentResolver
    {
        /// <summary>
        /// Searches for <see cref="XamlRoot"/> of given object. 
        /// Returns <see langword="null"/> if no such could be found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">Object whose parent <see cref="XamlRoot"/> is to be found.</param>
        public static XamlRoot? FindParentXamlRoot<T>(object host) where T : DialogProperties, new()
        {
            if (host is INavigatable navigatable)
            {
                if (navigatable.Navigator is INavigatorGetHostView getHostView)
                {
                    if(getHostView.HostView is UIElement element)
                    {
                        return element.XamlRoot;
                    }
                }
            }

            if (host is IDataContextDialog<T> dialogBackend)
            {
                if (dialogBackend.DialogControl.View is UIElement element)
                {
                    return element.XamlRoot;
                }
            }

            if (host is Window window)
            {
                return window.Content?.XamlRoot;
            }

            return null;
        }

        /// <summary>
        /// Searches for parent <see cref="Window"/> of provided object.
        /// Returns <see langword="null"/> if no such could be found.
        /// </summary>
        /// <typeparam name="T">Type of statically typed properties of dialog module which initiated search process.</typeparam>
        /// <param name="host">Object whose parent is to be found.</param>
        public static Window? FindParentWindow<T>(object host) where T : DialogProperties, new()
        {
            if (host is INavigatable navigatable)
            {
                if (navigatable.Navigator is INavigatorGetHostView getHostView)
                {
                    object? hostView = getHostView.HostView;
                    UIElement? topElement = FindTopElement(hostView);
                    return FindWindowWithContent(topElement);
                }
            }

            if (host is IDataContextDialog<T> dialogBackend)
            {
                return dialogBackend.DialogControl?.View as Window;
            }

            return host as Window;
        }

        private static Window? FindWindowWithContent(UIElement? topElement)
        {
            if (topElement is null)
            {
                return null;
            }

            foreach (IDialogModule dialog in DialogManager.GetOpenedDialogs())
            {
                if (dialog.View is Window window)
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
            if (root is null)
            {
                return null;
            }

            object currentElement = root;
            while (currentElement is FrameworkElement element)
            {
                if (element.Parent is DependencyObject)
                {
                    return element;
                }

                currentElement = element.Parent;
            }

            return null;
        }
    }
}
