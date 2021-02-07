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
    /// Provides a typical implementation of <see cref="IDialogModule"/> for WPF apps.
    /// </summary>
    public class CustomDialogModule : IDialogModule
    {
        protected Func<Window, Window> _getParentWindow;
        protected Window _view;
        protected IDialog _dataContext;
        protected bool _isOpen = false;

        /// <summary>
        /// Returns a reference to underlying <see cref="Window"/> object.
        /// </summary>
        public virtual object View => _view;

        /// <summary>
        /// An <see cref="IDialog"/> object bounded to <see cref="View"/>
        /// instance by *DataBinding* mechanism.
        /// </summary>
        public virtual IDialog DataContext => _dataContext;

        /// <summary>
        /// Specifies whether this dialog is currently open.
        /// </summary>
        public virtual bool IsOpen => _isOpen;

        /// <summary>
        /// Fires when dialog closes.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Fires when dialog is disposed and *DataContext* on <see cref="View"/> object is cleared.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Returns a new instance of a <see cref="CustomDialogModule"/> class.
        /// </summary>
        /// <param name="window">Dialog window.</param>
        /// <param name="dialog">Dialog backend; will be binded to dialog window by *DataBinding* mechanism.</param>
        public CustomDialogModule(Window window, IDialog dialog) : this(window, dialog, null) { }

        /// <summary>
        /// Returns a new instance of a <see cref="CustomDialogModule"/> class.
        /// </summary>
        /// <param name="window">Dialog window.</param>
        /// <param name="dialog">Dialog backend; will be binded to dialog window by *DataBinding* mechanism.</param>
        /// <param name="getParentWindow">
        /// A delegate which returns parent window of this dialog.
        /// This might be called on non-UI-thread; make sure you access application
        /// resources on appropriate thread.
        /// </param>
        public CustomDialogModule(Window window, IDialog dialog, Func<Window, Window> getParentWindow)
        {
            _view = window;
            _getParentWindow = getParentWindow;
            SetDataContext(dialog);

            window.Closed += (s, e) =>
            {
                _isOpen = false;
                OnClose();
            };

            window.Loaded += (s, e) => _isOpen = true;
        }

        /// <summary>
        /// Closes the dialog if open.
        /// </summary>
        public virtual void Close()
        {
            if(_isOpen)
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
            _dataContext = dialog;
            _view.DataContext = dialog;
        }

        /// <summary>
        /// Displays the dialog in non-modal manner.
        /// </summary>
        public virtual void Show()
        {
            _view.Owner = _getParentWindow?.Invoke(_view);
            _view.Show();
        }

        /// <summary>
        /// Displays the dialog asynchronously in non-modal manner.
        /// </summary>
        public virtual Task ShowAsync()
        {
            return Task.Run(() => 
            {
                if(_getParentWindow != null)
                {
                    Window parent = _getParentWindow.Invoke(_view);
                    parent.Dispatcher.Invoke(() => 
                    {
                        Show();
                    });
                }
                else
                {
                    Show();
                }
            });
        }

        /// <summary>
        /// Displays the dialog in modal manner.
        /// </summary>
        public virtual bool? ShowModal()
        {
            _view.Owner = _getParentWindow?.Invoke(_view);
            bool? result = _view.ShowDialog();
            _dataContext.DialogResult = result;
            return result;
        }

        /// <summary>
        /// Displays the dialog asynchronously in modal manner.
        /// </summary>
        /// <returns></returns>
        public virtual Task<bool?> ShowModalAsync()
        {
            return Task.Run(() =>
            {
                if (_getParentWindow != null)
                {
                    bool? result = null;

                    Window parent = _getParentWindow.Invoke(_view);
                    parent.Dispatcher.Invoke(() =>
                    {
                        result = ShowModal();
                    });

                    return result;
                }
                else
                {
                    return ShowModal();
                }
            });
        }

        /// <summary>
        /// Perform cleaning operations allowing this object to be garbage collected.
        /// </summary>
        public virtual void Dispose()
        {
            if(_view != null)
            {
                _view.DataContext = null;
            }

            OnDisposed();
        }

        protected virtual void OnClose()
        {
            _isOpen = false;
            Closed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDisposed()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }


    }
}
