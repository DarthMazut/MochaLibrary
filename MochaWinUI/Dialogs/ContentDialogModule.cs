using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MochaCore.Dialogs;

namespace MochaWinUI.Dialogs
{
    /// <summary>
    /// Provides implementation of <see cref="ICustomDialogModule"/> for WinUI3 <see cref="ContentDialog"/>.
    /// </summary>
    public class ContentDialogModule : ICustomDialogModule
    {
        protected readonly ContentDialog _view;
        private IDataContextDialog? _dataContext;
        private bool _isDialogOpen;
        private bool _isDisposed;
        private bool _isManualResult;
        private bool? _manualResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule"/> class.
        /// </summary>
        /// <param name="contentDialog">Technology-specific representation of this dialog module</param>
        public ContentDialogModule(ContentDialog contentDialog) : this(contentDialog, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule"/> class.
        /// </summary>
        /// <param name="contentDialog">Technology-specific representation of this dialog module</param>
        /// <param name="dataContext">
        /// A dialog logic bound to view object by *DataBinding* mechanism.
        /// Passing <see langword="null"/> means that the DataContext from the provided view object will be used, 
        /// as long as it's of type <see cref="IDataContextDialog"/>. Otherwise, the DataContext will be <see langword="null"/>. 
        /// </param>
        public ContentDialogModule(ContentDialog contentDialog, IDataContextDialog? dataContext)
        {
            _view = contentDialog;
            _dataContext = SelectDataContext(contentDialog, dataContext);
            _view.Opened += DialogOpened;
            _view.Closing += DialogClosing;

            FindParent = FindParentCore;
            HandleResult = HandleResultCore;
            DisposeModule = DisposeModuleCore;
        }

        /// <inheritdoc/>
        public object View => _view;

        /// <inheritdoc/>
        public IDataContextDialog? DataContext => _dataContext;

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

        /// <summary>
        /// Returns the <see cref="XamlRoot"/> for a technology-specific dialog object.
        /// Throws an exception if the XamlRoot could not be found.
        /// <para>
        /// Use this delagate to avoiding subcalssing only for overriding <see cref="FindParentCore(object?)"/>.
        /// Setting this delegate overrides default <see cref="FindParentCore(object?)"/> implementation.
        /// </para>
        /// </summary>
        public Func<object?, XamlRoot> FindParent { get; init; }

        /// <summary>
        /// Translates technology-specific dialog result object into technology-independent <see langword="bool?"/> value.
        /// <para>
        /// Use this delagate to avoiding subcalssing only for overriding <see cref="HandleResultCore(ContentDialogResult)"/>.
        /// Setting this delegate overrides default<see cref="HandleResultCore(ContentDialogResult)"/> implementation.
        /// </para>
        /// </summary>
        public Func<ContentDialogResult, bool?> HandleResult { get; init; }

        /// <summary>
        /// Allows to define additional code to be invoked while this module is being disposed.
        /// <para>
        /// Use this delagate to avoiding subcalssing only for overriding <see cref="DisposeModuleCore"/>.
        /// Setting this delegate overrides default <see cref="DisposeModuleCore"/> implementation.
        /// </para>
        /// </summary>
        public Action<ContentDialogModule> DisposeModule { get; init; }

        /// <inheritdoc/>
        public void Close(bool? result)
        {
            _isManualResult = true;
            _manualResult = result;
            _view.Hide();
        }

        /// <inheritdoc/>
        public void SetDataContext(IDataContextDialog? dataContext)
        {
            ThrowIfOpened();
            ThrowIfDisposed();

            _dataContext = dataContext;
        }

        /// <inheritdoc/>
        public async Task<bool?> ShowModalAsync(object? host)
        {
            ApplyPropertiesOverride();
            AssignDataContextIfRequired();
            _view.XamlRoot = FindParent(host);
            InitializeDataContext(_dataContext);
            Opening?.Invoke(this, EventArgs.Empty);
            ContentDialogResult rawResult = await DisplayDialog(_view);
            bool? typedResult = HandleResultCore(rawResult);
            Closed?.Invoke(this, EventArgs.Empty);
            return typedResult;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // TODO: It should be configurable, whether disposing a module should also
            // dispose DataContext

            if (_dataContext is IDisposable disposable)
            {
                disposable.Dispose();
            }

            DisposeModule(this);
            _view.DataContext = null;
            _view.Opened -= DialogOpened;
            _view.Closing -= DialogClosing;

            GC.SuppressFinalize(this);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void ApplyPropertiesOverride() { }

        /// <summary>
        /// Initializes a *DialogControl* object associated with the given dataContext
        /// by calling the <see cref="IDialogControlInitialize.Initialize(IDataContextDialogModule)"/>
        /// method.
        /// </summary>
        /// <param name="dataContext">The data context associated with the current instance.</param>
        protected virtual void InitializeDataContext(IDataContextDialog? dataContext)
        {
            if (dataContext is IDataContextDialog baseDialog)
            {
                if (baseDialog.DialogControl is IDialogControlInitialize dialogInitialize)
                {
                    dialogInitialize.Initialize(this);
                }
            }
        }

        /// <summary>
        /// Returns the <see cref="XamlRoot"/> for a technology-specific dialog object.
        /// Throws an exception if the XamlRoot could not be found.
        /// </summary>
        /// <param name="host">The host object for which to find the parent XamlRoot.</param>
        /// <returns>The <see cref="XamlRoot"/> associated with the given host object.</returns>
        /// <exception cref="NotImplementedException">Thrown if the XamlRoot could not be found for the provided host object.</exception>
        protected virtual XamlRoot FindParentCore(object? host)
        {
            if (host is UIElement uiElement)
            {
                return uiElement.XamlRoot;
            }

            if (host is Window window)
            {
                return window.Content.XamlRoot;
            }

            throw new NotImplementedException($"The current implementation of {GetType().Name} was unable to locate " +
                $"the xaml root for the dialog it was supposed to display. Please try providing your own " +
                $"implementation of the {nameof(FindParent)} method.");
        }

        /// <summary>
        /// Defines how the technology-specific dialog object represented by this module should be displayed.
        /// </summary>
        /// <param name="view">The actual dialog object to be displayed.</param>
        protected virtual Task<ContentDialogResult> DisplayDialog(ContentDialog view) => view.ShowAsync().AsTask();

        /// <summary>
        /// Translates technology-specific dialog result object into technology-independent <see langword="bool?"/> value.
        /// </summary>
        /// <param name="result">Technology-specific <see cref="ContentDialog"/> result object.</param>
        protected virtual bool? HandleResultCore(ContentDialogResult result) => result switch
        {
            ContentDialogResult.None => null,
            ContentDialogResult.Primary => true,
            ContentDialogResult.Secondary => false,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// Allows to define additional code to be invoked while this module is being disposed.
        /// </summary>
        protected virtual void DisposeModuleCore(ContentDialogModule module) { }

        private IDataContextDialog? SelectDataContext(ContentDialog contentDialog, IDataContextDialog? dataContext)
        {
            if (dataContext is not null)
            {
                return dataContext;
            }

            if (contentDialog.DataContext is null)
            {
                return null;
            }

            return contentDialog.DataContext is IDataContextDialog typedContext
                ? typedContext
                : throw new ArgumentException($"DataContext assigned to technology-specific dialog object must be of type {nameof(IDataContextDialog)}", nameof(dataContext));
        }

        private void AssignDataContextIfRequired()
        {
            if (_view.DataContext != _dataContext)
            {
                _view.DataContext = _dataContext;
            }
        }

        private bool? HandleResultBase(ContentDialogResult result)
        {
            if (_isManualResult)
            {
                _isManualResult = false;
                return _manualResult;
            }

            return HandleResult(result);
        }

        private void DialogClosing(ContentDialog sender, ContentDialogClosingEventArgs e)
        {
            CancelEventArgs args = new();
            Closing?.Invoke(this, args);
            e.Cancel = args.Cancel;
        }

        private void DialogOpened(ContentDialog sender, ContentDialogOpenedEventArgs e) => Opened?.Invoke(this, EventArgs.Empty);

        private void ThrowIfOpened()
            => throw new InvalidOperationException("Cannot perform the operation because the dialog is opened.");

        private void ThrowIfDisposed()
            => throw new InvalidOperationException("Cannot perform the operation because the module has already been disposed.");
    }

    public class ContentDialogModule<T> : ContentDialogModule, ICustomDialogModule<T> where T : new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule"/> class.
        /// </summary>
        /// <param name="contentDialog">Technology-specific representation of this dialog module</param>
        public ContentDialogModule(ContentDialog contentDialog) : this(contentDialog, null, new()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule"/> class.
        /// </summary>
        /// <param name="contentDialog">Technology-specific representation of this dialog module</param>
        /// <param name="dataContext">
        /// A dialog logic bound to view object by *DataBinding* mechanism.
        /// Passing <see langword="null"/> means that the DataContext from the provided view object will be used, 
        /// as long as it's of type <see cref="IDataContextDialog"/>. Otherwise, the DataContext will be <see langword="null"/>. 
        /// </param>
        public ContentDialogModule(ContentDialog contentDialog, IDataContextDialog? dataContext) : this(contentDialog, dataContext, new()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDialogModule"/> class.
        /// </summary>
        /// <param name="contentDialog">Technology-specific representation of this dialog module</param>
        /// <param name="dataContext">
        /// A dialog logic bound to view object by *DataBinding* mechanism.
        /// Passing <see langword="null"/> means that the DataContext from the provided view object will be used, 
        /// as long as it's of type <see cref="IDataContextDialog"/>. Otherwise, the DataContext will be <see langword="null"/>. 
        /// </param>
        /// <param name="properties"></param>
        public ContentDialogModule(ContentDialog contentDialog, IDataContextDialog? dataContext, T properties) : base(contentDialog, dataContext)
        {
            Properties = properties;
        }

        /// <inheritdoc/>
        public T Properties { get; set; }

        /// <summary>
        /// Customizes view object based on <see cref="Properties"/>.
        /// </summary>
        /// <param name="properties">Properties object which serves as a source for customization.</param>
        /// <param name="dialog">View object to be customized.</param>
        protected virtual void ApplyProperties(T properties, ContentDialog dialog) { }

        /// <inheritdoc/>
        protected override sealed void ApplyPropertiesOverride() => ApplyProperties(Properties, _view);
    }


    /*
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
    */
}
