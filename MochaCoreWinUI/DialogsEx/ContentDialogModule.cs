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
    public class ContentDialogModule<T> : ICustomDialogModule<T>
    {
        private bool _isOpen = false;
        private bool _wasClosed = false;
        protected Window _mainWindow;
        protected ContentDialog _view;
        protected ICustomDialog<T>? _dataContext;

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
        public ContentDialogModule(Window mainWindow, ContentDialog view, ICustomDialog<T> dataContext) : this(mainWindow, view, dataContext, default(T)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule{T}"/> class.
        /// </summary>
        /// <param name="mainWindow">Main application window object.</param>
        /// <param name="view">Instance of <see cref="ContentDialog"/> or its descendant.</param>
        /// <param name="dataContext">Datacontext for view object.</param>
        /// <param name="properties">Properties object associated with this instance.</param>
        public ContentDialogModule(Window mainWindow, ContentDialog view, ICustomDialog<T> dataContext, T properties)
        {
            _mainWindow = mainWindow;
            _view = view;

            SetDataContext(dataContext);

            if (properties is null)
            {
                if (typeof(T).GetConstructor(Array.Empty<Type>()) != null)
                {
                    Properties = (T)Activator.CreateInstance(typeof(T))!;
                }
            }
            else
            {
                Properties = properties;
            }

            view.Opened += (s, e) => Opened?.Invoke(this, EventArgs.Empty);
            view.Closing += (s, e) =>
            {
                CancelEventArgs cancelEventArgs = new();
                Closing?.Invoke(this, cancelEventArgs);
                e.Cancel = cancelEventArgs.Cancel;
            };

            ApplyProperties = ApplyPropertiesCore;
            HandleResult = HandleResultCore;
            FindParent = FindParentCore;
            DisposeDialog = DisposeDialogCore;
        }

        /// <inheritdoc/>
        public object? View => _view;

        /// <inheritdoc/>
        public ICustomDialog<T>? DataContext => _dataContext;

        /// <inheritdoc/>
        public T Properties { get; set; }

        /// <summary>
        /// Customizes view object based on properties within <see cref="Properties"/>.
        /// Use this delagate to avoiding subcalssing only for overriding <see cref="ApplyPropertiesCore(T?, ContentDialog)"/>.
        /// <para>Setting this delegate overrides default <c>ApplyPropertiesCore()</c> implementation.</para>
        /// </summary>
        public Action<T, ContentDialog> ApplyProperties { get; set; }

        /// <summary>
        /// Translates technology-specific dialog result object into technology-independent <see langword="bool?"/> value.
        /// Sets the dialog results within <see cref="Properties"/> object.
        /// Use this delagate to avoiding subcalssing only for overriding <see cref="HandleResultCore(ContentDialogResult, ContentDialog, T?)"/>.
        /// <para>Setting this delegate overrides default <c>HandleResultCore()</c> implementation.</para>
        /// </summary>
        public Func<ContentDialogResult, ContentDialog, T, bool?> HandleResult { get; set; }

        /// <summary>
        /// Resolves parent <see cref="XamlRoot"/> of provided object.
        /// </summary>
        public Func<object, XamlRoot> FindParent { get; set; }

        /// <summary>
        /// Allows to define additional code to be invoked while this module is being disposed.
        /// </summary>
        public Action<ContentDialogModule<T>> DisposeDialog { get; set; }

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
            if (_dataContext is IDialogInitialize dialogInitialize)
            {
                dialogInitialize.Uninitialize();
            }
            _view.DataContext = null;

            DisposeDialogCore(this);

            GC.SuppressFinalize(this);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void SetDataContext(ICustomDialog<T>? dataContext)
        {
            _dataContext = dataContext;
            _view.DataContext = dataContext;

            if(dataContext is not null)
            {
                dataContext.DialogModule = this;
                if (dataContext is IDialogInitialize dialogInitialize)
                {
                    dialogInitialize.Initialize();
                }
            }
        }

        /// <inheritdoc/>
        public async Task<bool?> ShowModalAsync(object host)
        {
            ApplyProperties.Invoke(Properties, _view);
            Opening?.Invoke(this, EventArgs.Empty);
            _view.XamlRoot = FindParentCore(host);
            _isOpen = true;
            _wasClosed = false;
            bool? result = HandleResult.Invoke(await _view.ShowAsync(), _view, Properties);
            _isOpen = false;
            OnClose();
            return result;
        }

        /// <summary>
        /// Allows for providing a custom code to be executed while this object is being disposed of.
        /// Override this when there are disposable resources within your custom <see cref="Properties"/> object.
        /// </summary>
        /// <param name="module">Module that's being disposed.</param>
        protected virtual void DisposeDialogCore(ContentDialogModule<T> module) { }

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
        protected virtual XamlRoot FindParentCore(object host)
        {
            XamlRoot? parentRoot = ParentResolver.FindParentXamlRoot<T>(host);
            if (parentRoot is null)
            {
                parentRoot = _mainWindow.Content.XamlRoot;
            }

            return parentRoot;
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
