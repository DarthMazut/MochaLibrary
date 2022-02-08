using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MochaCore.DialogsEx;
using MochaCore.Navigation;
using Windows.UI.Core;

namespace MochaCoreWinUI.DialogsEx
{
    public class ContentDialogModule<T> : IUserDialogModule<T>, IDialogClose
    {
        private bool _isOpen = false;
        private bool _wasClosed = false;
        protected Window _mainWindow;
        protected ContentDialog _view;
        protected XamlRoot? _parent;
        protected IDialog<T>? _dataContext;

        public ContentDialogModule(Window mainWindow, ContentDialog view) : this(mainWindow, view, null, default(T)) { }

        public ContentDialogModule(Window mainWindow, ContentDialog view, IDialog<T> dataContext) : this(mainWindow, view, dataContext, default(T)) { }

        public ContentDialogModule(Window mainWindow, ContentDialog view, IDialog<T>? dataContext, T? properties)
        {
            _mainWindow = mainWindow;
            _view = view;
            if (dataContext is not null)
            {
                SetDataContext(dataContext);
            }

            Properties = properties;

            view.Opened += (s, e) => Opened?.Invoke(this, EventArgs.Empty);
            view.Closing += (s, e) =>
            {
                CancelEventArgs cancelEventArgs = new();
                Closing?.Invoke(this, cancelEventArgs);
                e.Cancel = cancelEventArgs.Cancel;
            };
        }

        public object? View => _view;

        public object? Parent => _parent;

        public IDialog<T>? DataContext => _dataContext;

        public T Properties { get; set; }

        public Action<T, ContentDialog>? ApplyPropertiesDelegate { get; set; }

        public Func<ContentDialogResult, ContentDialog, T, bool?>? HandleResultDelegate { get; set; }

        public event EventHandler? Opening;
        public event EventHandler? Closed;
        public event EventHandler? Disposed;
        public event EventHandler? Opened;
        public event EventHandler<CancelEventArgs>? Closing;

        public void Close()
        {
            if (_isOpen)
            {
                _view.Hide();
                OnClose();
            }
        }

        public void Dispose()
        {
            DisposeCore();
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public void SetDataContext(IDialog<T> dataContext)
        {
            _view.DataContext = dataContext;
            dataContext.DialogModule = this;
            _dataContext = dataContext;
        }

        public async Task<bool?> ShowModalAsync(object host)
        {
            ApplyProperties();
            _parent = FindParent(host);
            _view.XamlRoot = _parent;
            Opening?.Invoke(this, EventArgs.Empty);
            _isOpen = true;
            _wasClosed = false;
            bool? result = HandleResult(await _view.ShowAsync());
            _isOpen = false;
            OnClose();
            return result;
        }

        protected virtual void DisposeCore() 
        {
            _view.DataContext = null;
        }

        protected virtual bool? HandleResultCore(ContentDialogResult contentDialogResult, ContentDialog view, T? properties)
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

        protected virtual void ApplyPropertiesCore(T? properties, ContentDialog view) { }

        protected virtual XamlRoot FindParent(object host)
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

        private bool? HandleResult(ContentDialogResult contentDialogResult)
        {
            if (HandleResultDelegate is not null)
            {
                return HandleResultDelegate?.Invoke(contentDialogResult, _view, Properties);
            }
            else
            {
                return HandleResultCore(contentDialogResult, _view, Properties);
            }
        }

        private void ApplyProperties()
        {
            if (ApplyPropertiesDelegate is not null)
            {
                ApplyPropertiesDelegate?.Invoke(Properties, _view);
            }
            else
            {
                ApplyPropertiesCore(Properties, _view);
            }
        }

        private void OnClose()
        {
            if (!_wasClosed)
            {
                _wasClosed = true;
                Closed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
