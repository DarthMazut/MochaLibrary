using Mocha.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MochaWPF
{
    /// <summary>
    /// Provides a base class for an implementation of <see cref="IDialogModule"/>. 
    /// </summary>
    public abstract class BaseDialogModule : IDialogModule
    {
        private IDialog _dataContext;
        private bool _isDisposed;

        /// <summary>
        /// Reference to WPF application.
        /// </summary>
        protected Application Application { get; }

        /// <summary>
        /// Return view object associated with represented dialog.
        /// </summary>
        public abstract object View { get; }

        /// <summary>
        /// An <see cref="IDialog"/> object bounded to <see cref="View"/>
        /// instance by *DataBinding* mechanism.
        /// </summary>
        public IDialog DataContext => _dataContext;

        /// <summary>
        /// Specifies whether this dialog is currently open.
        /// </summary>
        public bool IsOpen { get; protected set; }


        /// <summary>
        /// Fires when dialog closes.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Fires when dialog is done.
        /// <para>"Comrade soldier, you're done!"</para>
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Base constructor for derived types.
        /// </summary>
        /// <param name="app">Reference to WPF application.</param>
        public BaseDialogModule(Application app)
        {
            Application = app;
        }

        /// <summary>
        /// Called when dialog closes.
        /// </summary>
        protected void OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when dialog is disposed.
        /// </summary>
        protected void OnDisposed()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Closes the dialog if open.
        /// </summary>
        public void Close()
        {
            if(IsOpen)
            {
                CloseCore();
                IsOpen = false;
                OnClosed();
            }
        }

        /// <summary>
        /// Perform cleaning operations allowing this object to be garbage collected.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            else
            {
                DisposeCore();
                _isDisposed = true;
                OnDisposed();
            }
        }

        /// <summary>
        /// Sets a * DataContext * for <see cref="View"/> object.
        /// </summary>
        /// <param name="dialog">Dialog logic to be set by * DataContext * mechanism.</param>
        public void SetDataContext(IDialog dialog)
        {
            SetDataContextCore(dialog);
            _dataContext = dialog;
        }

        /// <summary>
        /// Opens a dialog represented by this instance.
        /// </summary>
        public void Show()
        {
            EnsureDialogCanBeShown();
            IsOpen = true;
            ShowCore();
        }

        /// <summary>
        /// Opens a dialog represented by this instance in asynchronous manner.
        /// </summary>
        public Task ShowAsync()
        {
            EnsureDialogCanBeShown();
            IsOpen = true;
            return ShowCoreAsync();
        }

        /// <summary>
        /// Opens a dialog represented by this instance in modal mode. 
        /// Returns result of dialog interaction.
        /// </summary>
        public bool? ShowModal()
        {
            EnsureDialogCanBeShown();
            IsOpen = true;
            return ShowModalCore();
        }

        /// <summary>
        /// Asynchronously opens a dialog represented by this instance in modal mode. 
        /// Returns result of dialog interaction.
        /// </summary>
        public Task<bool?> ShowModalAsync()
        {
            EnsureDialogCanBeShown();
            IsOpen = true;
            return ShowModalCoreAsync();
        }

        /// <summary>
        /// Returns a <see cref="Window"/> which is logical parent to represented dialog.
        /// This value is calucalted from <see cref="DialogParameters.Parent"/>.
        /// </summary>
        protected virtual Window GetParentWindow()
        {
            Window parent = null;

            Application.Dispatcher.Invoke(() =>
            {
                List<IDialogModule> modules = DialogManager.GetActiveDialogs();
                IDialog parentDialog = _dataContext.DialogParameters.Parent;

                IDialogModule parentModule = modules.Where(m => m.DataContext == parentDialog).FirstOrDefault();

                foreach (Window window in Application.Windows)
                {
                    if (window == parentModule?.View)
                    {
                        parent = window;
                        return;
                    }
                }

                parent = Application.Windows[0];
            });

            return parent;
        }

        /// <summary>
        /// Performs closing action.
        /// </summary>
        protected abstract void CloseCore();

        /// <summary>
        /// Performs cleaning operations. It is important to set 
        /// *DataContex* of view object to <see langword="null"/> here.
        /// </summary>
        protected abstract void DisposeCore();

        /// <summary>
        /// Sets provided <see cref="IDialog"/> data as a *DataContext* for
        /// view object.
        /// </summary>
        /// <param name="dialog">Backend for represented dialog.</param>
        protected abstract void SetDataContextCore(IDialog dialog);

        /// <summary>
        /// Show dialog here.
        /// </summary>
        protected virtual void ShowCore()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Show dialog here in asynchronous manner.
        /// </summary>
        protected virtual Task ShowCoreAsync()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Show dialog in modal mode here.
        /// </summary>
        protected virtual bool? ShowModalCore()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Show dialog in modal mode in asynchronous manner here.
        /// </summary>
        protected virtual Task<bool?> ShowModalCoreAsync()
        {
            throw new NotSupportedException();
        }

        private void EnsureDialogCanBeShown()
        {
            if (IsOpen)
            {
                throw new InvalidOperationException($"{GetType()} was already opened");
            }

            if(_isDisposed)
            {
                throw new InvalidOperationException("Cannot show disposed dialog.");
            }
        }
    }
}
