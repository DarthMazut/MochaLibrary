using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MochaCore.Dialogs;

namespace MochaWinUI.Dialogs
{
    public class TestModule : ICustomDialogModule
    {
        public ICustomDialog? DataContext => throw new NotImplementedException();

        public object? View => throw new NotImplementedException();

        public event EventHandler Opening;
        public event EventHandler Closed;
        public event EventHandler Disposed;
        public event EventHandler Opened;
        public event EventHandler<CancelEventArgs> Closing;

        public void Close(bool? result)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SetDataContext(ICustomDialog? dataContext)
        {
            throw new NotImplementedException();
        }

        public Task<bool?> ShowModalAsync(object host)
        {
            throw new NotImplementedException();
        }
    }

    public class TestModule<T> : ICustomDialogModule<T> where T : new()
    {
        public ICustomDialog<T>? DataContext => throw new NotImplementedException();

        public T Properties { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public object? View => throw new NotImplementedException();

        //ICustomDialog? ICustomDialogModule.DataContext => throw new NotImplementedException();

        //IDataContextDialog? IDataContextDialogModule.DataContext => throw new NotImplementedException();

        public event EventHandler Opened;
        public event EventHandler<CancelEventArgs> Closing;
        public event EventHandler Opening;
        public event EventHandler Closed;
        public event EventHandler Disposed;

        public void Close(bool? result)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SetDataContext(ICustomDialog<T>? dataContext)
        {
            throw new NotImplementedException();
        }

        public void SetDataContext(ICustomDialog? dataContext)
        {
            throw new NotImplementedException();
        }

        public void SetDataContext(IDataContextDialog? dataContext)
        {
            throw new NotImplementedException();
        }

        public Task<bool?> ShowModalAsync(object host)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Provides base implementation for WinUI 3 <see cref="ContentDialog"/>-based modules.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="Properties"/> object.</typeparam>
    public class ContentDialogModule<T> : ICustomDialogModule<T> where T : new()
    {
        private bool _wasClosed;
        private bool? _manualCloseResult;
        protected Window _mainWindow;
        protected ContentDialog _view;
        protected ICustomDialog<T>? _dataContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule{T}"/> class.
        /// </summary>
        /// <param name="mainWindow">Application main window object.</param>
        /// <param name="view">Technology-specific representation of this dialog module (<see cref="ContentDialog"/> or its descendant).</param>
        public ContentDialogModule(Window mainWindow, ContentDialog view) : this(mainWindow, view, null, new T()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule{T}"/> class.
        /// </summary>
        /// <param name="mainWindow">Application main window object.</param>
        /// <param name="view">Technology-specific representation of this dialog module (<see cref="ContentDialog"/> or its descendant).</param>
        /// <param name="dataContext">
        /// A dialog logic bound to view object by DataBinding mechanism.
        /// Passing <see langword="null"/> means that the DataContext from the provided view object will be used, 
        /// as long as it's of type <see cref="ICustomDialog{T}"/>. Otherwise, the DataContext will be <see langword="null"/>. 
        /// </param>
        public ContentDialogModule(Window mainWindow, ContentDialog view, ICustomDialog<T>? dataContext) : this(mainWindow, view, dataContext, new T()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule{T}"/> class.
        /// </summary>
        /// <param name="mainWindow">Application main window object.</param>
        /// <param name="view">Technology-specific representation of this dialog module (<see cref="ContentDialog"/> or its descendant).</param>
        /// <param name="dataContext">
        /// A dialog logic bound to view object by DataBinding mechanism.
        /// Passing <see langword="null"/> means that the DataContext from the provided view object will be used, 
        /// as long as it's of type <see cref="ICustomDialog{T}"/>. Otherwise, the DataContext will be <see langword="null"/>. 
        /// </param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        public ContentDialogModule(Window mainWindow, ContentDialog view, ICustomDialog<T>? dataContext, T properties)
        {
            _mainWindow = mainWindow;
            _view = view;

            Properties = properties;

            if (dataContext is null && view.DataContext is ICustomDialog<T> dialogDataContext)
            {
                _dataContext = dialogDataContext;
                dialogDataContext.DialogControl.Initialize(this);
            }
            else
            {
                SetDataContext(dataContext);
            }

            view.Opened += DialogOpened;
            view.Closing += DialogClosing;

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
        public void Close(bool? result)
        {
            _view.Hide();
            _wasClosed = true;
            _manualCloseResult = result;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _dataContext?.DialogControl?.Dispose();

            if (_dataContext is IDisposable disposable)
            {
                disposable.Dispose();
            }
            
            DisposeDialog?.Invoke(this);
            _view.DataContext = null;
            _dataContext = null;
            _view.Opened -= DialogOpened;
            _view.Closing -= DialogClosing;

            GC.SuppressFinalize(this);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void SetDataContext(ICustomDialog<T>? dataContext)
        {
            _dataContext = dataContext;
            _view.DataContext = dataContext;
            dataContext?.DialogControl.Initialize(this);
        }

        /// <inheritdoc/>
        public async Task<bool?> ShowModalAsync(object host)
        {
            ApplyProperties.Invoke(Properties, _view);
            Opening?.Invoke(this, EventArgs.Empty);
            _view.XamlRoot = FindParentCore(host);
            
            bool? result = HandleResult.Invoke(await _view.ShowAsync(), _view, Properties);
            if (_wasClosed)
            {
                result = _manualCloseResult;
            }

            Closed?.Invoke(this, EventArgs.Empty);
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

        private void DialogOpened(ContentDialog sender, ContentDialogOpenedEventArgs e)
        {
            Opened?.Invoke(this, EventArgs.Empty);
        }

        private void DialogClosing(ContentDialog sender, ContentDialogClosingEventArgs e)
        {
            CancelEventArgs cancelEventArgs = new();
            Closing?.Invoke(this, cancelEventArgs);
            e.Cancel = cancelEventArgs.Cancel;
        }
    }

    /// <summary>
    /// Provides base implementation for WinUI 3 <see cref="ContentDialog"/>-based modules.
    /// </summary>
    public class ContentDialogModule : ContentDialogModule<DialogProperties>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule{T}"/> class.
        /// </summary>
        /// <param name="mainWindow">Application main window object.</param>
        /// <param name="view">Technology-specific representation of this dialog module (<see cref="ContentDialog"/> or its descendant).</param>
        public ContentDialogModule(Window mainWindow, ContentDialog view) : base(mainWindow, view) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule{T}"/> class.
        /// </summary>
        /// <param name="mainWindow">Application main window object.</param>
        /// <param name="view">Technology-specific representation of this dialog module (<see cref="ContentDialog"/> or its descendant).</param>
        /// <param name="dataContext">
        /// A dialog logic bound to view object by DataBinding mechanism.
        /// Passing <see langword="null"/> means that the DataContext from the provided view object will be used, 
        /// as long as it's of type <see cref="ICustomDialog{T}"/>. Otherwise, the DataContext will be <see langword="null"/>. 
        /// </param>
        public ContentDialogModule(Window mainWindow, ContentDialog view, ICustomDialog<DialogProperties>? dataContext) : base(mainWindow, view, dataContext) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule{T}"/> class.
        /// </summary>
        /// <param name="mainWindow">Application main window object.</param>
        /// <param name="view">Technology-specific representation of this dialog module (<see cref="ContentDialog"/> or its descendant).</param>
        /// <param name="dataContext">
        /// A dialog logic bound to view object by DataBinding mechanism.
        /// Passing <see langword="null"/> means that the DataContext from the provided view object will be used, 
        /// as long as it's of type <see cref="ICustomDialog{T}"/>. Otherwise, the DataContext will be <see langword="null"/>. 
        /// </param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        public ContentDialogModule(Window mainWindow, ContentDialog view, ICustomDialog<DialogProperties>? dataContext, DialogProperties properties) : base(mainWindow, view, dataContext, properties) { }
    }
}
