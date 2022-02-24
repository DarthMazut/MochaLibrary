using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
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
    /// Provides standard implementation of <see cref="IDialogModule{T}"/> for WPF MessageBox.
    /// </summary>
    public class StandardMessageDialogModule : IDialogModule<StandardMessageDialogProperties>
    {
        private readonly Window _mainWindow;
        private Window? _parent;

        public StandardMessageDialogModule(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public object? View => null;
        public object? Parent => _parent;
        public StandardMessageDialogProperties Properties { get; set; } = new();

        public Func<StandardMessageDialogProperties, MessageBoxResult>? ShowDialogDelegate { get; set; }

        public Func<MessageBoxResult, StandardMessageDialogProperties, bool?>? HandleResultDelegate { get; set; }

        public Func<StandardMessageDialogProperties, MessageBoxButton>? ResolveButtonsDelegate { get; set; }

        public Func<StandardMessageDialogProperties, MessageBoxImage>? ResolveIconDelegate { get; set; }

        public event EventHandler? Opening;
        public event EventHandler? Closed;
        public event EventHandler? Disposed;

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public Task<bool?> ShowModalAsync(object host)
        {
            Opening?.Invoke(this, EventArgs.Empty);
            _parent = FindParent(host);
            bool? result = HandleResult(ShowDialog());
            _parent = null;
            Closed?.Invoke(this, EventArgs.Empty);
            return Task.FromResult(result);
        }

        private bool? HandleResult(MessageBoxResult messageBoxResult)
        {
            if (HandleResultDelegate is not null)
            {
                return HandleResultDelegate?.Invoke(messageBoxResult, Properties);
            }
            else
            {
                return HandleResultCore(messageBoxResult, Properties);
            }
        }

        protected virtual bool? HandleResultCore(MessageBoxResult messageBoxResult, StandardMessageDialogProperties properties)
        {
            switch (messageBoxResult)
            {
                case MessageBoxResult.None:
                    return null;
                case MessageBoxResult.OK:
                    return true;
                case MessageBoxResult.Cancel:
                    return null;
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                default:
                    return null;
            }
        }

        protected virtual MessageBoxResult ShowDialogCore(StandardMessageDialogProperties properties)
        {
            return MessageBox.Show(_parent, properties.Message, properties.Title, ResolveButtons(properties), ResolveIcon(properties));
        }

        protected virtual MessageBoxButton ResolveButtonsCore(StandardMessageDialogProperties properties)
        {
            bool hasDeclineButton = properties.DeclineButtonText is not null;
            bool hasCancelButton = properties.CancelButtonText is not null;

            if (hasDeclineButton && hasCancelButton)
            {
                return MessageBoxButton.YesNoCancel;
            }

            if (hasDeclineButton)
            {
                return MessageBoxButton.YesNo;
            }

            if (hasCancelButton)
            {
                return MessageBoxButton.OKCancel;
            }

            return MessageBoxButton.OK;
        }

        protected virtual MessageBoxImage ResolveIconCore(StandardMessageDialogProperties properties)
        {
            if (properties.Icon == StandardMessageDialogIcons.Error) return MessageBoxImage.Error;
            if (properties.Icon == StandardMessageDialogIcons.Information) return MessageBoxImage.Information;
            if (properties.Icon == StandardMessageDialogIcons.Question) return MessageBoxImage.Question;
            if (properties.Icon == StandardMessageDialogIcons.Warning) return MessageBoxImage.Warning;

            return MessageBoxImage.None;
        }

        private MessageBoxResult ShowDialog()
        {
            if (ShowDialogDelegate is not null)
            {
                return ShowDialogDelegate.Invoke(Properties);
            }
            else
            {
                return ShowDialogCore(Properties);
            }
        }

        private MessageBoxButton ResolveButtons(StandardMessageDialogProperties properties)
        {
            if (ResolveButtonsDelegate is not null)
            {
                return ResolveButtonsDelegate.Invoke(properties);
            }
            else
            {
                return ResolveButtonsCore(properties);
            }
        }

        private MessageBoxImage ResolveIcon(StandardMessageDialogProperties properties)
        {
            if (ResolveIconDelegate is not null)
            {
                return ResolveIconDelegate.Invoke(properties);
            }
            else
            {
                return ResolveIconCore(properties);
            }
        }

        private Window FindParent(object host)
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

            return _mainWindow;
        }

        private Window? TraverseVisualTreeToFindWindow(object root)
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
