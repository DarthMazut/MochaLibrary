using MochaCore.Dialogs;
using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MochWPF.Dialogs
{
    /// <summary>
    /// Provides a default algorithm for searching parent window for given object.
    /// </summary>
    public static class ParentResolver
    {
        /// <summary>
        /// Tries to find parent <see cref="Window"/> for provided object by traversing tree of WPF visual elements.
        /// Returns <see langword="null"/> if no parent window can be found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">Object which parent window is to be found.</param>
        public static Window? FindParent<T>(object host) where T : DialogProperties, new()
        {
            Window? foundWindow;
            if (host is FrameworkElement hostFrameworkElement)
            {
                foundWindow = TraverseVisualTreeToFindWindow(hostFrameworkElement);
                if (foundWindow is not null)
                {
                    return foundWindow;
                }
            }

            if (host is IDataContextDialog<T> dialog && dialog.DialogControl.View is FrameworkElement viewFrameworkElement)
            {
                foundWindow = TraverseVisualTreeToFindWindow(viewFrameworkElement);
                if (foundWindow is not null)
                {
                    return foundWindow;
                }
            }

            //if (host is ICustomDialog<T> customDlg && customDlg.DialogModule.View is FrameworkElement customViewFrameworkElement)
            //{
            //    foundWindow = TraverseVisualTreeToFindWindow(customViewFrameworkElement);
            //    if (foundWindow is not null)
            //    {
            //        return foundWindow;
            //    }
            //}

            if (host is INavigatable navigatable)
            {
                foundWindow = TraverseVisualTreeToFindWindow((navigatable.Navigator as INavigatorGetHostView).HostView!);
                if (foundWindow is not null)
                {
                    return foundWindow;
                }
            }

            return null;
        }

        private static Window? TraverseVisualTreeToFindWindow(object root)
        {
            object currentElement = root;
            while (true)
            {
                if (currentElement is Window foundWindow)
                {
                    return foundWindow;
                }
                else if (currentElement is FrameworkElement foundElement)
                {
                    currentElement = foundElement.Parent;
                    continue;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
