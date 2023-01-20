using MochaCore.DialogsEx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace MochCoreWPF.DialogsEx
{
    /// <summary>
    /// Provides standard implementation of <see cref="ICustomDialogModule{T}"/> for WPF <see cref="Window"/> object.
    /// </summary>
    /// <typeparam name="T">Type of statically typed properties object used for configuration of this module.</typeparam>
    public class WindowDialogModule<T> : ICustomDialogModule<T>, IDialogClose where T : DialogProperties, new()
    {
        private readonly Window _dialogWindow;
        private ICustomDialog<T>? _dataContext;

        private readonly CancelEventHandler _onClosing;
        private readonly RoutedEventHandler _onOpened;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowDialogModule{T}"/> class.
        /// </summary>
        /// <param name="dialogWindow">Technology-specific representation of this dialog module.</param>
        public WindowDialogModule(Window dialogWindow) : this(dialogWindow, null, new T()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowDialogModule{T}"/> class.
        /// </summary>
        /// <param name="dialogWindow">Technology-specific representation of this dialog module.</param>
        /// <param name="dataContext">
        /// A dialog logic bound to view object by DataBinding mechanism.
        /// Passing <see langword="null"/> means that the DataContext from the provided view object will be used, 
        /// as long as it's of type <see cref="ICustomDialog{T}"/>. Otherwise, the DataContext will be <see langword="null"/>. 
        /// </param>
        public WindowDialogModule(Window dialogWindow, ICustomDialog<T>? dataContext) : this(dialogWindow, dataContext, new T()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowDialogModule{T}"/> class.
        /// </summary>
        /// <param name="dialogWindow">Technology-specific representation of this dialog module.</param>
        /// <param name="dataContext">
        /// A dialog logic bound to view object by DataBinding mechanism.
        /// Passing <see langword="null"/> means that the DataContext from the provided view object will be used, 
        /// as long as it's of type <see cref="ICustomDialog{T}"/>. Otherwise, the DataContext will be <see langword="null"/>. 
        /// </param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        public WindowDialogModule(Window dialogWindow, ICustomDialog<T>? dataContext, T properties)
        {
            _ = dialogWindow ?? throw new ArgumentNullException(nameof(dialogWindow));

            _dialogWindow = dialogWindow;
            Properties = properties;

            if (dataContext is null && dialogWindow.DataContext is ICustomDialog<T> dialogDataContext)
            {
                _dataContext = dialogDataContext;
                dialogDataContext.DialogControl.Initialize(this);
            }
            else
            {
                SetDataContext(dataContext);
            }

            dialogWindow.Closing += _onClosing = (s, e) => Closing?.Invoke(this, e);
            dialogWindow.Loaded += _onOpened = (s, e) => Opened?.Invoke(this, EventArgs.Empty);

            FindParent = FindParentCore;
            ApplyProperties = ApplyPropertiesCore;
            HandleResult = HandleResultCore;
            DisposeDialog = DisposeDialogCore;
        }

        /// <inheritdoc/>
        public object? View => _dialogWindow;

        /// <inheritdoc/>
        public T Properties { get; set; }

        /// <summary>
        /// Customizes view object based on properties within <see cref="Properties"/>.
        /// Use this delagate to avoiding subcalssing only for overriding <see cref="ApplyPropertiesCore(T?, ContentDialog)"/>.
        /// <para>Setting this delegate overrides default <c>ApplyPropertiesCore()</c> implementation.</para>
        /// </summary>
        public Action<Window, T> ApplyProperties { get; set; }

        /// <summary>
        /// Translates technology-specific dialog result object into technology-independent <see langword="bool?"/> value.
        /// Use this delagate to avoiding subcalssing only for overriding <see cref="HandleResultCore(ContentDialogResult, ContentDialog, T?)"/>.
        /// <para>Setting this delegate overrides default <c>HandleResultCore()</c> implementation.</para>
        /// </summary>
        public Func<bool?, T, ICustomDialog<T>?, bool?> HandleResult { get; set; }

        /// <summary>
        /// Allows to define additional code to be invoked while this module is being disposed.
        /// </summary>
        public Action<ICustomDialogModule<T>>? DisposeDialog { get; set; }

        public Func<object, Window?> FindParent { get; set; }

        /// <inheritdoc/>
        public ICustomDialog<T> DataContext => _dataContext!;

        public event EventHandler? Opening;
        public event EventHandler? Closed;
        public event EventHandler? Disposed;
        public event EventHandler? Opened;
        public event EventHandler<CancelEventArgs>? Closing;

        /// <inheritdoc/>
        public void Close(bool? result)
        {
            _dialogWindow.DialogResult = result;
            _dialogWindow.Close();
            Closed?.Invoke(this, EventArgs.Empty);
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
            _dialogWindow.DataContext = null;
            _dataContext = null;
            _dialogWindow.Closing -= _onClosing;
            _dialogWindow.Loaded -= _onOpened;

            GC.SuppressFinalize(this);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void SetDataContext(ICustomDialog<T>? dataContext)
        {
            _dataContext = dataContext;
            _dialogWindow.DataContext = dataContext;
            dataContext?.DialogControl.Initialize(this);
        }

        /// <inheritdoc/>
        public Task<bool?> ShowModalAsync(object host)
        {
            ApplyProperties?.Invoke(_dialogWindow, Properties);
            Opening?.Invoke(this, EventArgs.Empty);
            _dialogWindow.Owner = FindParent.Invoke(host);
            bool? result = HandleResult.Invoke(_dialogWindow.ShowDialog(), Properties, DataContext);
            Closed?.Invoke(this, EventArgs.Empty);

            return Task.FromResult(result);
        }

        /// <summary>
        /// Customizes dialog window object based on <see cref="Properties"/>.
        /// </summary>
        /// <param name="dialogWindow">View object to be customized.</param>
        /// <param name="properties">Properties object which serves as a source for customization.</param>
        protected virtual void ApplyPropertiesCore(Window dialogWindow, T properties) { }

        /// <summary>
        /// Translates technology-specific dialog result object into technology-independent <see langword="bool?"/> value.
        /// </summary>
        /// <param name="result">Technology-specific result object.</param>
        /// <param name="view">Technology-specific dialog object.</param>
        /// <param name="properties">Reference to <see cref="Properties"/> object.</param>
        protected virtual bool? HandleResultCore(bool? result, T properties, ICustomDialog<T>? dataContext) => result;

        /// <summary>
        /// Allows for providing a custom code to be executed while this object is being disposed of.
        /// Override this when there are disposable resources within your custom <see cref="Properties"/> object.
        /// </summary>
        /// <param name="module">Module that's being disposed.</param>
        protected virtual void DisposeDialogCore(ICustomDialogModule<T>? module) { }

        /// <summary>
        /// Searches for technology-specific parent of host object.
        /// </summary>
        /// <param name="host">Object which technology-specific parent is to be found.</param>
        protected virtual Window? FindParentCore(object host)
        {
            return ParentResolver.FindParent<T>(host) ?? Application.Current.MainWindow;
        }
    }

    /// <summary>
    /// Provides standard implementation of <see cref="ICustomDialogModule{T}"/> for WPF <see cref="Window"/> object.
    /// </summary>
    public sealed class WindowDialogModule : WindowDialogModule<DialogProperties>, IDialogClose
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowDialogModule"/> class.
        /// </summary>
        /// <param name="dialogWindow">Technology-specific representation of this dialog module.</param>
        public WindowDialogModule(Window dialogWindow) : base(dialogWindow) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowDialogModule"/> class.
        /// </summary>
        /// <param name="dialogWindow">Technology-specific representation of this dialog module.</param>
        /// <param name="dataContext">A dialog logic bound to view object by DataBinding mechanism.</param>
        public WindowDialogModule(Window dialogWindow, ICustomDialog<DialogProperties> dataContext) : base(dialogWindow, dataContext) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowDialogModule"/> class.
        /// </summary>
        /// <param name="dialogWindow">Technology-specific representation of this dialog module.</param>
        /// <param name="dataContext">A dialog logic bound to view object by DataBinding mechanism.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        public WindowDialogModule(Window dialogWindow, ICustomDialog<DialogProperties> dataContext, DialogProperties properties) : base(dialogWindow, dataContext, properties) { }
    }
}
