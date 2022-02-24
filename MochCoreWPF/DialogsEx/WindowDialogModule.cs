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
    public class WindowDialogModule<T> : IUserDialogModule<T>, IDialogClose
    {
        private readonly Window _mainWindow;
        private readonly Window _dialogWindow;

        private Window? _parent;
        private IDialog<T>? _dataContext;

        public WindowDialogModule(Window mainWindow, Window dialogWindow, IDialog<T> dataContext, T properties)
        {
            _ = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            _ = dialogWindow ?? throw new ArgumentNullException(nameof(dialogWindow));

            _mainWindow = mainWindow;
            _dialogWindow = dialogWindow;

            SetDataContext(dataContext);
            Properties = properties;

            dialogWindow.Closing += (s, e) => this.Closing?.Invoke(this, e);
            dialogWindow.Loaded += (s, e) => this.Opened?.Invoke(this, EventArgs.Empty);

            FindParent = (host) => ParentResolver.FindParent(host) ?? _mainWindow;
        }

        public object? View => _mainWindow;

        public object? Parent => _parent;

        public T Properties { get; set; }

        public IDialog<T>? DataContext => _dataContext;

        public Action<Window, T>? ApplyProperties { get; set; }

        public Func<bool?, T, IDialog<T>?, bool?> HandleResult { get; set; } = (result, properties, dataContext) => result;

        public Action<IDialog<T>?>? DisposeCore { get; set; }

        public Func<object, Window?> FindParent { get; set; }

        public event EventHandler? Opening;
        public event EventHandler? Closed;
        public event EventHandler? Disposed;
        public event EventHandler? Opened;
        public event EventHandler<CancelEventArgs>? Closing;

        public void Close()
        {
            _dialogWindow.Close();
            Closed?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _dialogWindow.DataContext = null;
            DisposeCore?.Invoke(DataContext);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public void SetDataContext(IDialog<T> dataContext)
        {
            _dataContext = dataContext;
            _dataContext.DialogModule = this;
            _dialogWindow.DataContext = dataContext;
        }

        public Task<bool?> ShowModalAsync(object host)
        {
            ApplyProperties?.Invoke(_dialogWindow, Properties);
            Opening?.Invoke(this, EventArgs.Empty);
            _parent = FindParent.Invoke(host);
            _dialogWindow.Owner = _parent;
            bool? result = HandleResult.Invoke(_dialogWindow.ShowDialog(), Properties, DataContext);
            _parent = null;
            Closed?.Invoke(this, EventArgs.Empty);

            return Task.FromResult(result);
        }
    }
}
