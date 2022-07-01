using MochaCore.DialogsEx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public WindowDialogModule(Window dialogWindow) : this(dialogWindow, null, default(T)) { }

        public WindowDialogModule(Window dialogWindow, ICustomDialog<T> dataContext) : this(dialogWindow, dataContext, default(T)) { }

        public WindowDialogModule(Window dialogWindow, ICustomDialog<T> dataContext, T properties)
        {
            _ = dialogWindow ?? throw new ArgumentNullException(nameof(dialogWindow));

            _dialogWindow = dialogWindow;
            SetDataContext(dataContext);
            Properties = properties ?? new T();

            dialogWindow.Closing += (s, e) => this.Closing?.Invoke(this, e);
            dialogWindow.Loaded += (s, e) => this.Opened?.Invoke(this, EventArgs.Empty);

            FindParent = FindParentCore;
            ApplyProperties = ApplyPropertiesCore;
            HandleResult = HandleResultCore;
            DisposeDialog = DisposeDialogCore;
        }

        /// <inheritdoc/>
        public object? View => _dialogWindow;

        /// <inheritdoc/>
        public T Properties { get; set; }

        public Action<Window, T> ApplyProperties { get; set; }

        public Func<bool?, T, ICustomDialog<T>?, bool?> HandleResult { get; set; }

        /// <summary>
        /// Allows to define additional code to be invoked while this module is being disposed.
        /// </summary>
        public Action<ICustomDialog<T>?>? DisposeDialog { get; set; }

        public Func<object, Window?> FindParent { get; set; }

        public ICustomDialog<T> DataContext => _dataContext!;

        public event EventHandler? Opening;
        public event EventHandler? Closed;
        public event EventHandler? Disposed;
        public event EventHandler? Opened;
        public event EventHandler<CancelEventArgs>? Closing;

        /// <inheritdoc/>
        public void Close()
        {
            _dialogWindow.Close();
            Closed?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            if (_dataContext is IDialogInitialize dialogInitialize)
            {
                dialogInitialize.Uninitialize();
            }

            DisposeDialog?.Invoke(DataContext);
            _dialogWindow.DataContext = null;
            
            GC.SuppressFinalize(this);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public void SetDataContext(ICustomDialog<T>? dataContext)
        {
            _dataContext = dataContext;
            _dialogWindow.DataContext = dataContext;

            if (dataContext is not null)
            {
                dataContext.DialogModule = this;
                if (dataContext is IDialogInitialize dialogInitialize)
                {
                    dialogInitialize.Initialize();
                }
            }
        }

        public Task<bool?> ShowModalAsync(object host)
        {
            ApplyProperties?.Invoke(_dialogWindow, Properties);
            Opening?.Invoke(this, EventArgs.Empty);
            _dialogWindow.Owner = FindParent.Invoke(host);
            bool? result = HandleResult.Invoke(_dialogWindow.ShowDialog(), Properties, DataContext);
            Closed?.Invoke(this, EventArgs.Empty);

            return Task.FromResult(result);
        }

        protected virtual void ApplyPropertiesCore(Window dialogWindow, T properties) { }

        protected virtual bool? HandleResultCore(bool? result, T properties, ICustomDialog<T>? dataContext)
        {
            return result;
        }

        /// <summary>
        /// Allows for providing a custom code to be executed while this object is being disposed of.
        /// Override this when there are disposable resources within your custom <see cref="Properties"/> object.
        /// </summary>
        /// <param name="module">Module that's being disposed.</param>
        protected virtual void DisposeDialogCore(ICustomDialog<T>? module) { }

        protected virtual Window? FindParentCore(object host)
        {
            return ParentResolver.FindParent<T>(host) ?? Application.Current.MainWindow;
        }
    }
}
