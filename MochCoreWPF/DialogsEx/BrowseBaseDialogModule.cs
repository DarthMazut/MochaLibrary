using MochaCore.DialogsEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MochCoreWPF.DialogsEx
{
    public abstract class BrowseBaseDialogModule<TView, TResult, TProperties> : IDialogModule<TProperties>
    {
        private readonly Window _mainWindow;
        private readonly TView _view;
        private Window? _parent;

        public BrowseBaseDialogModule(Window mainWindow, TView view)
        {
            _mainWindow = mainWindow;
            _view = view;

            FindParent = (host) => ParentResolver.FindParent<TProperties>(host) ?? _mainWindow;
        }

        public abstract TProperties Properties { get; set; }

        public object? View => _view;

        public object? Parent => _parent;

        public abstract Action<TView, TProperties> ApplyProperties { get; set; }

        public abstract Func<TView, Window, TResult> ShowModalCore { get; set; }

        public abstract Func<TView, TResult, TProperties, bool?> HandleResult { get; set; }

        public Func<object, Window> FindParent { get; set; }

        public event EventHandler? Opening;
        public event EventHandler? Closed;
        public event EventHandler? Disposed;

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public Task<bool?> ShowModalAsync(object host)
        {
            ApplyProperties.Invoke(_view, Properties);
            Opening?.Invoke(this, EventArgs.Empty);
            _parent = FindParent(host);
            bool? result = HandleResult(_view, ShowModalCore(_view, _parent), Properties);
            _parent = null;
            Closed?.Invoke(this, EventArgs.Empty);
            return Task.FromResult(result);
        }
    }
}
