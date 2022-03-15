using Microsoft.UI.Xaml;
using MochaCore.DialogsEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWinUI.DialogsEx
{
    public abstract class BrowseBaseDialogModule<TView, TResult, TProperties> : IDialogModule<TProperties>
    {
        private readonly Window _mainWindow;
        private readonly TView _view;

        public BrowseBaseDialogModule(Window mainWindow, TProperties properties, TView view)
        {
            _mainWindow = mainWindow;
            _view = view;
            Properties = properties;

            ApplyProperties = ApplyPropertiesCore;
            ShowDialog = ShowDialogCore;
            HandleResult = HandleResultCore;
            FindParent = FindParentCore;
            DisposeDialog = DisposeDialogCore;
        }

        public TProperties Properties { get; set; }

        public object? View => _view;

        /// <inheritdoc/>
        public event EventHandler? Opening;

        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        public Action<TView, TProperties> ApplyProperties { get; set; }

        public Func<TView, Window, Task<TResult>> ShowDialog { get; set; }

        public Func<TView, TResult, TProperties, bool?> HandleResult { get; set; }

        public Func<object, Window> FindParent { get; set; }

        public Action<BrowseBaseDialogModule<TView, TResult, TProperties>> DisposeDialog { get; set; }

        public void Dispose()
        {
            DisposeDialog.Invoke(this);
            GC.SuppressFinalize(this);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public async Task<bool?> ShowModalAsync(object host)
        {
            ApplyProperties.Invoke(_view, Properties);
            WorkaroundForBug466(FindParent.Invoke(host));
            
            Opening?.Invoke(this, EventArgs.Empty);
            bool? result = HandleResult.Invoke(_view, await ShowDialog.Invoke(_view, FindParent.Invoke(host)), Properties);
            Closed?.Invoke(this, EventArgs.Empty);

            return result;
        }

        protected abstract void ApplyPropertiesCore(TView dialog, TProperties properties);

        protected abstract Task<TResult> ShowDialogCore(TView dialog, Window parent);

        protected abstract bool? HandleResultCore(TView dialog, TResult result, TProperties properties);

        protected virtual Window FindParentCore(object host)
        {
            return _mainWindow; // wow!
        }

        protected virtual void DisposeDialogCore(BrowseBaseDialogModule<TView, TResult, TProperties> module) { }

        // Workaround for bug https://github.com/microsoft/WindowsAppSDK/issues/466
        private void WorkaroundForBug466(object parent)
        {
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(parent);
            WinRT.Interop.InitializeWithWindow.Initialize(_view, hwnd);
        }
    }
}
