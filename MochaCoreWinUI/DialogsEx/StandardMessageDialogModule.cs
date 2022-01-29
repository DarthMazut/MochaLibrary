using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWinUI.DialogsEx
{
    public class StandardMessageDialogModule : IDialogModule<StandardMessageDialogProperties>
    {
        protected readonly ContentDialog _view;
        private readonly Window _mainWindow;
        private object? _parent;

        public StandardMessageDialogModule(Window mainWindow, ContentDialog view)
        {
            _view = view;
            _mainWindow = mainWindow;
        }

        public object? View => _view;
        public object? Parent => _parent;
        public StandardMessageDialogProperties Properties { get; set; } = new("Title","content...");

        public Action<StandardMessageDialogProperties, ContentDialog>? ApplyPropertiesDelegate { get; set; }

        public Func<ContentDialogResult, StandardMessageDialogProperties, bool?>? HandleResultDelegate { get; set; }

        public event EventHandler? Opening;
        public event EventHandler? Closed;
        public event EventHandler? Disposed;

        public void Dispose() 
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public async Task<bool?> ShowModalAsync(object host)
        {
            ApplyPropertiesCore();
            _view.XamlRoot = FindParent(host);
            Opening?.Invoke(this, EventArgs.Empty);
            bool? result = HandleResultCore(await _view.ShowAsync());
            Closed?.Invoke(this, EventArgs.Empty);
            return result;
        }

        protected virtual void ApplyProperties()
        {
            _view.Title = Properties.Title;
            _view.Content = Properties.Message;
            _view.PrimaryButtonText = Properties.ConfirmationButtonText;
            _view.SecondaryButtonText = Properties?.DeclineButtonText;
            _view.CloseButtonText = Properties?.CancelButtonText;
        }

        protected virtual bool? HandleResult(ContentDialogResult contentDialogResult)
        {
            switch (contentDialogResult)
            {
                case ContentDialogResult.None:
                    return null;
                case ContentDialogResult.Primary:
                    return true;
                case ContentDialogResult.Secondary:
                    return false;
                default:
                    return null;
            }
        }

        private void ApplyPropertiesCore()
        {
            if (ApplyPropertiesDelegate is null)
            {
                ApplyProperties();
            }
            else
            {
                ApplyPropertiesDelegate.Invoke(Properties, _view);
            }
        }

        private bool? HandleResultCore(ContentDialogResult contentDialogResult)
        {
            if (HandleResultDelegate is null)
            {
                return HandleResult(contentDialogResult);
            }
            else
            {
                return HandleResultDelegate.Invoke(contentDialogResult, Properties);
            }
        }

        private XamlRoot FindParent(object host)
        {
            if (host is IDialog dialog)
            {
                if (dialog.DialogModule.View is Window window)
                {
                    return window.Content.XamlRoot;
                }

                if (dialog.DialogModule.View is UIElement uiElement)
                {
                    return uiElement.XamlRoot;
                }

                return _mainWindow.Content.XamlRoot;
            }
            
            if (host is INavigatable navigatable)
            {
                throw new NotImplementedException("Implement an explicit interface to retrieve a View object form INavigatable...");
            }

            return _mainWindow.Content.XamlRoot;
        }
    }
}
