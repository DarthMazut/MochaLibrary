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

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardMessageDialogModule"/> class.
        /// </summary>
        /// <param name="application">WPF application object.</param>
        public StandardMessageDialogModule(Application application)
        {
            _mainWindow = application.MainWindow;

            ShowDialog = ShowDialogCore;
            HandleResult = HandleResultCore;
            ResolveButtons = ResolveButtonsCore;
            ResolveIcon = ResolveIconCore;
            FindParent = FindParentCore;
        }

        /// <inheritdoc/>
        public object? View => null;

        /// <inheritdoc/>
        public StandardMessageDialogProperties Properties { get; set; } = new();

        /// <summary>
        /// A delegate which describes the process of showing dialog represented by this module.
        /// </summary>
        public Func<Window?, StandardMessageDialogProperties, MessageBoxResult> ShowDialog { get; set; }

        public Func<MessageBoxResult, StandardMessageDialogProperties, bool?> HandleResult { get; set; }

        public Func<StandardMessageDialogProperties, MessageBoxButton> ResolveButtons { get; set; }

        public Func<StandardMessageDialogProperties, MessageBoxImage> ResolveIcon{ get; set; }

        public Func<object, Window?> FindParent { get; set; }

        /// <inheritdoc/>
        public event EventHandler? Opening;

        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public Task<bool?> ShowModalAsync(object host)
        {
            Opening?.Invoke(this, EventArgs.Empty);
            bool? result = HandleResult.Invoke(ShowDialog.Invoke(FindParent.Invoke(host), Properties), Properties);
            Closed?.Invoke(this, EventArgs.Empty);
            return Task.FromResult(result);
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

        protected virtual MessageBoxResult ShowDialogCore(Window? host, StandardMessageDialogProperties properties)
        {
            return MessageBox.Show(host, properties.Message, properties.Title, ResolveButtons.Invoke(properties), ResolveIcon.Invoke(properties));
        }

        protected virtual Window? FindParentCore(object host)
        {
            return ParentResolver.FindParent<StandardMessageDialogProperties>(host) ?? _mainWindow;
        }
    }
}
