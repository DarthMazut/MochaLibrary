using MochaCore.DialogsEx;
using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MochCoreWPF.DialogsEx
{
    /// <summary>
    /// Provides a default algorithm for searching parent window for given object.
    /// </summary>
    internal static class ParentResolver
    {
        /// <summary>
        /// Tries to find parent <see cref="Window"/> for provided object by traversing tree of WPF visual elements.
        /// Returns <see langword="null"/> if no parent window can be found.
        /// </summary>
        /// <param name="host">Object which parent window is to be found.</param>
        public static Window? FindParent(object host)
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

            if (host is IDialog dialog && dialog.DialogModule.View is FrameworkElement viewFrameworkElement)
            {
                foundWindow = TraverseVisualTreeToFindWindow(viewFrameworkElement);
                if (foundWindow is not null)
                {
                    return foundWindow;
                }
            }

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
