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
    /// <summary>
    /// Provides base implementation for WinUI 3 <see cref="ContentDialog"/>-based modules.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="Properties"/> object.</typeparam>
    public class ContentDialogModule<T> : ICustomDialogModule<T>, IDialogClose
    {
        private bool _isOpen = false;
        private bool _wasClosed = false;
        protected Window _mainWindow;
        protected ContentDialog _view;
        protected XamlRoot? _parent;
        protected IDialog<T>? _dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule{T}"/> class.
        /// </summary>
        /// <param name="mainWindow">Main application window object.</param>
        /// <param name="view">Instance of <see cref="ContentDialog"/> or its descendant.</param>
        public ContentDialogModule(Window mainWindow, ContentDialog view) : this(mainWindow, view, null, default(T)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule{T}"/> class.
        /// </summary>
        /// <param name="mainWindow">Main application window object.</param>
        /// <param name="view">Instance of <see cref="ContentDialog"/> or its descendant.</param>
        /// <param name="dataContext">Datacontext for view object.</param>
        public ContentDialogModule(Window mainWindow, ContentDialog view, IDialog<T> dataContext) : this(mainWindow, view, dataContext, default(T)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule{T}"/> class.
        /// </summary>
        /// <param name="mainWindow">Main application window object.</param>
        /// <param name="view">Instance of <see cref="ContentDialog"/> or its descendant.</param>
        /// <param name="dataContext">Datacontext for view object.</param>
        /// <param name="properties">Properties object associated with this instance.</param>
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

        /// <inheritdoc/>
        public object? View => _view;

        /// <inheritdoc/>
        public object? Parent => _parent;

        /// <inheritdoc/>
        public IDialog<T>? DataContext => _dataContext;

        /// <inheritdoc/>
        public T Properties { get; set; }

        /// <summary>
        /// Customizes view object based on properties within <see cref="Properties"/>.
        /// Use this delagate to avoiding subcalssing only for overriding <see cref="ApplyPropertiesCore(T?, ContentDialog)"/>.
        /// <para>Setting this delegate overrides default <c>ApplyPropertiesCore()</c> implementation.</para>
        /// </summary>
        public Action<T, ContentDialog>? ApplyPropertiesDelegate { get; set; }

        /// <summary>
        /// Translates technology-specific dialog result object into technology-independent <see langword="bool?"/> value.
        /// Sets the dialog results within <see cref="Properties"/> object.
        /// Use this delagate to avoiding subcalssing only for overriding <see cref="HandleResultCore(ContentDialogResult, ContentDialog, T?)"/>.
        /// <para>Setting this delegate overrides default <c>HandleResultCore()</c> implementation.</para>
        /// </summary>
        public Func<ContentDialogResult, ContentDialog, T, bool?>? HandleResultDelegate { get; set; }

        /// <inheritdoc/>
        public event EventHandler? Opening;

        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <inheritdoc/>
        public event EventHandler? Opened;

        /// <inheritdoc/>
        public event EventHandler<CancelEventArgs>? Closing;

        /// <inheritdoc/>
        public void Close()
        {
            if (_isOpen)
            {
                _view.Hide();
                OnClose();
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            DisposeCore();
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void SetDataContext(IDialog<T> dataContext)
        {
            _view.DataContext = dataContext;
            dataContext.DialogModule = this;
            _dataContext = dataContext;
        }

        /// <inheritdoc/>
        public async Task<bool?> ShowModalAsync(object host)
        {
            ApplyProperties();
            Opening?.Invoke(this, EventArgs.Empty);
            _parent = FindParent(host);
            _view.XamlRoot = _parent;
            _isOpen = true;
            _wasClosed = false;
            bool? result = HandleResult(await _view.ShowAsync());
            _isOpen = false;
            _parent = null;
            OnClose();
            return result;
        }

        /// <summary>
        /// Sets *DataContex* of <see cref="View"/> object to <see langword="null"/>.
        /// </summary>
        protected virtual void DisposeCore() 
        {
            _view.DataContext = null;
        }

        /// <summary>
        /// Translates technology-specific dialog result object into technology-independent <see langword="bool?"/> value.
        /// Sets the dialog results within <see cref="Properties"/> object.
        /// </summary>
        /// <param name="contentDialogResult">Technology-specific <see cref="ContentDialog"/> result object.</param>
        /// <param name="view">Technology-specific dialog object.</param>
        /// <param name="properties">Reference to <see cref="Properties"/> object.</param>
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

        /// <summary>
        /// Customizes view object based on <see cref="Properties"/>.
        /// </summary>
        /// <param name="properties">Properties object which serves as a source for customization.</param>
        /// <param name="view">View object to be customized.</param>
        protected virtual void ApplyPropertiesCore(T? properties, ContentDialog view) { }

        /// <summary>
        /// Searches for technology-specific parent of host object.
        /// </summary>
        /// <param name="host">Object which technology-specific parent is to be found.</param>
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
