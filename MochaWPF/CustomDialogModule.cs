using Mocha.Dialogs;
using System;
using System.Collections.Generic;
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
        protected Application _application;
        protected Window _view;
        protected IDialog _dataContext;
        protected bool _isOpen = false;
        private bool _isDisposed = false;

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
        /// Returns a new instance of <see cref="CustomDialogModule"/> class.
        /// </summary>
        /// <param name="application">A reference to WPF <see cref="Application"/> object.</param>
        /// <param name="window">A <see cref="Window"/> which will be associated with the <see cref="CustomDialogModule"/> being created.</param>
        /// <param name="dataContext">A default dialog logic bounded to <see cref="CustomDialogModule"/> by *DataContext* mechanism.</param>
        public CustomDialogModule(Application application, Window window, IDialog dataContext)
        {
            _application = application;
            _view = window;
            SetDataContext(dataContext);

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
            if(_isOpen)
            {
                throw new InvalidOperationException($"{_view.GetType()} was already opened");
            }

            _view.Owner = GetParentWindow();
            _view.Show();
        }

        /// <summary>
        /// Displays the dialog asynchronously in non-modal manner.
        /// </summary>
        public virtual Task ShowAsync()
        {
            return Task.Run(() =>
            {
                _application.Dispatcher.Invoke(() =>
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
            if (_isOpen)
            {
                throw new InvalidOperationException($"{_view.GetType()} was already opened");
            }

            _view.Owner = GetParentWindow();
            bool? result = _view.ShowDialog();
            _dataContext.DialogResult = result;
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

                _application.Dispatcher.Invoke(() =>
                {
                    result = ShowModal();
                });

                return result;
            });
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
                Disposed?.Invoke(this, EventArgs.Empty);
                _isDisposed = true;
            }
        }

        /// <summary>
        /// Rises <see cref="Closed"/> event.
        /// </summary>
        protected virtual void OnClose()
        {
            _isOpen = false;
            Closed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Rises <see cref="Disposed"/> event.
        /// </summary>
        protected virtual void OnDisposed()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Returns parent <see cref="Window"/> based on value from <see cref="IDialog.Parameters"/>.
        /// </summary>
        protected virtual Window GetParentWindow()
        {
            Window parent = null;

            _application.Dispatcher.Invoke(() => 
            {
                List<IDialogModule> modules = DialogManager.GetActiveDialogs();
                IDialog parentDialog = _dataContext.Parameters.Parent;

                IDialogModule parentModule = modules.Where(m => m.DataContext == parentDialog).FirstOrDefault();

                foreach (Window window in _application.Windows)
                {
                    if(window == parentModule?.View)
                    {
                        parent = window;
                        return;
                    }
                }

                parent = _application.Windows[0];
            });

            return parent;
        }
    }
}
