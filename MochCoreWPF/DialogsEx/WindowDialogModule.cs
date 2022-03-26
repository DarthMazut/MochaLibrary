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
    public class WindowDialogModule<T> : ICustomDialogModule<T>, IDialogClose
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
            _dialogWindow.DataContext = null;
            DisposeDialog?.Invoke(DataContext);
            GC.SuppressFinalize(this);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public void SetDataContext(ICustomDialog<T>? dataContext)
        {
            _dataContext = dataContext;

            if (dataContext is not null)
            {
                dataContext.DialogModule = this;
            }

            _dialogWindow.DataContext = dataContext;
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

        protected virtual void DisposeDialogCore(ICustomDialog<T>? module) { }

        protected virtual Window? FindParentCore(object host)
        {
            return ParentResolver.FindParent<T>(host) ?? Application.Current.MainWindow;
        }
    }
}
