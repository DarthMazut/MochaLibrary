using Mocha.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MochaWPF
{
    /// <summary>
    /// Provides a typical implementation of <see cref="IDialogModule"/> for WPF apps.
    /// </summary>
    public class CustomDialogModule : IDialogModule
    {
        private Window _view;

        /// <summary>
        /// A reference to WPF <see cref="System.Windows.Application"/> object.
        /// </summary>
        protected Application Application { get; set; }

        /// <summary>
        /// An <see cref="IDialog"/> object bounded to <see cref="View"/>
        /// instance by *DataBinding* mechanism.
        /// </summary>
        public IDialog DataContext { get; protected set; }

        /// <summary>
        /// Determines whether dialog has been disposed.
        /// </summary>
        protected bool IsDisposed { get; set; }

        /// <summary>
        /// Returns a reference to underlying <see cref="Window"/> object.
        /// </summary>
        public virtual object View => _view;

        /// <summary>
        /// Specifies whether this dialog is currently open.
        /// </summary>
        public bool IsOpen { get; protected set; }

        public event EventHandler<CancelEventArgs> Closing;

        /// <summary>
        /// Fires when dialog closes.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Fires when dialog is disposed and *DataContext* on <see cref="View"/> object is cleared.
        /// </summary>
        public event EventHandler Disposed;
        
        /// <summary>
        /// Returns a new instance of <see cref="CustomDialogModule"/> class.
        /// </summary>
        /// <param name="application">A reference to WPF <see cref="Application"/> object.</param>
        /// <param name="window">A <see cref="Window"/> which will be associated with created <see cref="CustomDialogModule"/>.</param>
        /// <param name="dataContext">A default dialog logic bounded to <see cref="CustomDialogModule"/> by *DataContext* mechanism.</param>
        public CustomDialogModule(Application application, Window window, IDialog dataContext)
        {
            _view = window;
            Application = application;
            SetDataContext(dataContext);

            window.Closed += (s, e) =>
            {
                IsOpen = false;
                OnClose();
            };

            window.Loaded += (s, e) => IsOpen = true;
        }

        /// <summary>
        /// Called when dialog closes.
        /// </summary>
        protected virtual void OnClose()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when dialog is disposed.
        /// </summary>
        protected virtual void OnDisposed()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Closes the dialog if open.
        /// </summary>
        public virtual void Close()
        {
            if(IsOpen)
            {
                _view.Close();
            }
        }

        /// <summary>
        /// Sets a * DataContext * for <see cref="View"/> object.
        /// </summary>
        /// <param name="dialog">Dialog logic to be set by * DataContext * mechanism.</param>
        public virtual void SetDataContext(IDialog dialog)
        {
            _view.DataContext = dialog;
            DataContext = dialog;
            SetBehaviors(this.Close, DataContext);
        }

        /// <summary>
        /// Displays the dialog in non-modal manner.
        /// </summary>
        public virtual void Show()
        {
            if(IsOpen)
            {
                throw new InvalidOperationException($"{_view.GetType()} was already opened");
            }

            if (IsDisposed)
            {
                throw new InvalidOperationException("Cannot show already disposed dialog");
            }

            _view.Owner = DialogModuleHelper.GetParentWindow(Application, DataContext);
            _view.Show();
        }

        /// <summary>
        /// Displays the dialog asynchronously in non-modal manner.
        /// </summary>
        public virtual Task ShowAsync()
        {
            return Task.Run(() =>
            {
                Application.Dispatcher.Invoke(() =>
                {
                    Show();
                });
            });
        }

        /// <summary>
        /// Displays the dialog in modal manner.
        /// </summary>
        public virtual bool? ShowModal()
        {
            if (IsOpen)
            {
                throw new InvalidOperationException($"{_view.GetType()} was already opened");
            }

            if (IsDisposed)
            {
                throw new InvalidOperationException("Cannot show already disposed dialog");
            }

            _view.Owner = DialogModuleHelper.GetParentWindow(Application, DataContext);
            bool? result = _view.ShowDialog();
            DataContext.DialogResult = result;
            return result;
        }

        /// <summary>
        /// Displays the dialog asynchronously in modal manner.
        /// </summary>
        public virtual Task<bool?> ShowModalAsync()
        {
            return Task.Run(() =>
            {
                bool? result = null;

                Application.Dispatcher.Invoke(() =>
                {
                    result = ShowModal();
                });

                return result;
            });
        }

        /// <summary>
        /// Perform cleaning operations allowing this object to be garbage collected.
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            else
            {
                CleanUp();
                IsDisposed = true;
                Disposed?.Invoke(this, EventArgs.Empty);
                
            }
        }

        private void CleanUp()
        {
            _view.DataContext = null;
        }

        /// <summary>
        /// Sets up <see cref="DialogBehaviors"/> for given <see cref="IDialog"/> backend.
        /// </summary>
        /// <param name="closeAction">Implemented *Close* function.</param>
        /// <param name="dataContext">Dialog backend.</param>
        private void SetBehaviors(Action closeAction, IDialog dataContext)
        {
            DataContext.DialogActions = new DialogActions(this);
        }
    }
}
