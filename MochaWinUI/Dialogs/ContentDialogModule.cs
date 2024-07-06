using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MochaCore.Dialogs;
using MochaWinUI.Utils;

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
            DisplayDialog = DisplayDialogCore;
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
        /// Setting this delegate overrides default <see cref="HandleResultCore(ContentDialogResult)"/> implementation.
        /// </para>
        /// </summary>
        public Func<ContentDialogResult, bool?> HandleResult { get; init; }

        /// <summary>
        /// Defines how the technology-specific dialog object represented by this module should be displayed.
        /// <para>
        /// Use this delagate to avoiding subcalssing only for overriding <see cref="DisplayDialogCore(ContentDialog)"/>.
        /// Setting this delegate overrides default <see cref="DisplayDialogCore(ContentDialog)"/> implementation.
        /// </para>
        /// </summary>
        public Func<ContentDialog, Task<ContentDialogResult>> DisplayDialog { get; init; }

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
            bool? typedResult = HandleResult(rawResult);
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
            if (ParentResolver.FindParentXamlRoot(host) is XamlRoot foundRoot)
            {
                return foundRoot;
            }

            throw new NotImplementedException(
                $"The default implementation of {GetType().Name} could not resolve the parent of the provided object. " +
                $"In this case, you need to provide your own implementation of {nameof(FindParent)} either by supplying a custom " +
                $"{nameof(FindParent)} delegate or by subclassing {GetType().Name} and overriding the {nameof(FindParentCore)} method."
            );
        }

        /// <summary>
        /// Defines how the technology-specific dialog object represented by this module should be displayed.
        /// </summary>
        /// <param name="view">The actual dialog object to be displayed.</param>
        protected virtual Task<ContentDialogResult> DisplayDialogCore(ContentDialog view) => view.ShowAsync().AsTask();

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

    /// <summary>
    /// Provides implementation of <see cref="ICustomDialogModule"/> for WinUI3 <see cref="ContentDialog"/>.
    /// </summary>
    /// <typeparam name="T">Type of statically typed properties object used for configuration of this module.</typeparam>
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
            ApplyProperties = ApplyPropertiesCore;
        }

        /// <inheritdoc/>
        public T Properties { get; set; }

        /// <summary>
        /// Customizes view object based on <see cref="Properties"/>.
        /// <para>
        /// Use this delagate to avoiding subcalssing only for overriding <see cref="ApplyPropertiesCore(T, ContentDialog)"/>.
        /// Setting this delegate overrides default <see cref="ApplyPropertiesCore(T, ContentDialog)"/> implementation.
        /// </para>
        /// </summary>
        public Action<T, ContentDialog> ApplyProperties { get; init; }

        /// <summary>
        /// Customizes view object based on <see cref="Properties"/>.
        /// </summary>
        /// <param name="properties">Properties object which serves as a source for customization.</param>
        /// <param name="dialog">View object to be customized.</param>
        protected virtual void ApplyPropertiesCore(T properties, ContentDialog dialog) { }

        /// <inheritdoc/>
        protected override sealed void ApplyPropertiesOverride() => ApplyProperties(Properties, _view);
    }  
}
