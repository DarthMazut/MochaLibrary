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
    public class WindowDialogModule<T> : ICustomDialogModule<T>, IDialogClose
    {
        private readonly Window _mainWindow;
        private readonly Window _dialogWindow;

        private ICustomDialog<T>? _dataContext;

        public WindowDialogModule(Application application, Window dialogWindow, ICustomDialog<T> dataContext, T properties)
        {
            Window mainWindow = application.MainWindow;

            _ = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            _ = dialogWindow ?? throw new ArgumentNullException(nameof(dialogWindow));

            _mainWindow = mainWindow;
            _dialogWindow = dialogWindow;

            SetDataContext(dataContext);
            Properties = properties;

            dialogWindow.Closing += (s, e) => this.Closing?.Invoke(this, e);
            dialogWindow.Loaded += (s, e) => this.Opened?.Invoke(this, EventArgs.Empty);

            FindParent = (host) => ParentResolver.FindParent<T>(host) ?? _mainWindow;
        }

        public object? View => _mainWindow;

        public T Properties { get; set; }

        public Action<Window, T>? ApplyProperties { get; set; }

        public Func<bool?, T, ICustomDialog<T>?, bool?> HandleResult { get; set; } = (result, properties, dataContext) => result;

        public Action<ICustomDialog<T>?>? DisposeCore { get; set; }

        public Func<object, Window?> FindParent { get; set; }

        public ICustomDialog<T> DataContext => _dataContext!;

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

        public void SetDataContext(ICustomDialog<T> dataContext)
        {
            _dataContext = dataContext;
            _dataContext.DialogModule = this;
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
    }
}
