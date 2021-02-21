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
    /// Provides an implementation of windows <see cref="MessageBox"/> for WPF app.
    /// </summary>
    public class StandardDialogModule : IDialogModule
    {
        private IDialog _dataContext;
        private bool _isOpen = false;
        private Func<IDialogModule, Window> _getParentWindow;

        /// <summary>
        /// There is no view object which represents WPF <see cref="MessageBox"/> dialog
        /// therefore this always returns <see langword="null"/>.
        /// </summary>
        public object View => null;

        /// <summary>
        /// Returns an <see cref="IDialog"/> object which serve as a backend
        /// for represented <see cref="MessageBox"/>. 
        /// </summary>
        public IDialog DataContext => _dataContext;

        /// <summary>
        /// Indicates whether the represented <see cref="MessageBox"/> is currently open.
        /// </summary>
        public bool IsOpen => _isOpen;

        /// <summary>
        /// Fires when represented <see cref="MessageBox"/> closes.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Fires when this instance is done.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Default construtcor. Uses build-in simple implementaion of <see cref="IDialog"/>.
        /// </summary>
        public StandardDialogModule() : this(null, null) { }

        /// <summary>
        /// Returns new instance of <see cref="StandardDialogModule"/> class.
        /// </summary>
        /// <param name="dialogData">Backend for represented <see cref="MessageBox"/> dialog.</param>
        public StandardDialogModule(IDialog dialogData) : this(dialogData, null) { }

        /// <summary>
        /// Returns new instance of <see cref="StandardDialogModule"/> class.
        /// </summary>
        /// <param name="dialogData">Backend for represented <see cref="MessageBox"/> dialog.</param>
        /// <param name="getParentWindow">A delegate which returns parent window for this dialog.</param>
        public StandardDialogModule(IDialog dialogData, Func<IDialogModule, Window> getParentWindow)
        {
            _dataContext = dialogData ?? new SimpleDialog();
            _getParentWindow = getParentWindow;
        }

        /// <summary>
        /// This method is not supported by <see cref="StandardDialogModule"/> implementation.
        /// </summary>
        public void Close()
        {
            throw new NotSupportedException("Closing StandardDialogModule is currently not supported in MochaLib :(");
        }

        /// <summary>
        /// Marks this dialog as done.
        /// </summary>
        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sets backend data for this dialog.
        /// </summary>
        /// <param name="dialogData">Backend data to be set.</param>
        public void SetDataContext(IDialog dialogData)
        {
            _dataContext = dialogData;
        }

        /// <summary>
        /// Showing <see cref="MessageBox"/> in non-modal way is not supported by this implementation.
        /// </summary>
        public void Show()
        {
            throw new NotSupportedException("StandardDialogModule can only be displayed as modal.");
        }

        /// <summary>
        /// Showing <see cref="MessageBox"/> asynchronously in non-modal way is not supported by this implementation.
        /// </summary>
        public Task ShowAsync()
        {
            throw new NotSupportedException("StandardDialogModule can only be displayed as modal.");
        }

        /// <summary>
        /// Displays represented <see cref="MessageBox"/> dialog in modal manner.
        /// Returns result of dialog interaction.
        /// </summary>
        public bool? ShowModal()
        {
            _isOpen = true;
            bool? result = null;

            if (_getParentWindow != null)
            {
                Window parentWindow = _getParentWindow(this);
                result = HandleDialogDisplay(parentWindow, _dataContext.Parameters);
            }
            else
            {
                result = HandleDialogDisplay(null, _dataContext.Parameters);
            }

            OnClose();

            _dataContext.DialogResult = result;
            return result;
        }

        /// <summary>
        /// Displays asynchronously represented <see cref="MessageBox"/> dialog in modal manner.
        /// Returns result of dialog interaction.
        /// </summary>
        public Task<bool?> ShowModalAsync()
        {
            return Task.Run(() => 
            {
                bool? result = null;

                if (_getParentWindow != null)
                {
                    Window parent = _getParentWindow.Invoke(this);
                    parent.Dispatcher.Invoke(() =>
                    {
                        result = this.ShowModal();
                    });
                }
                else
                {
                    result = this.ShowModal();
                }

                return result;
            });
        }

        private void OnClose()
        {
            _isOpen = false;
            Closed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual bool? HandleDialogDisplay(Window parent, Dictionary<string, object> parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters was null.");

            string dialogTitle = string.Empty;
            string dialogContent = string.Empty;
            MessageBoxButton dialogButtons = MessageBoxButton.OK;
            MessageBoxImage dialogIcon = MessageBoxImage.None;

            if(parameters.TryGetValue(DialogParameters.Title, out object title))
            {
                if(title is string titleValue)
                {
                    dialogTitle = titleValue;
                }
            }

            if (parameters.TryGetValue(DialogParameters.Caption, out object caption))
            {
                if (caption is string captionValue)
                {
                    dialogContent = captionValue;
                }
            }

            if (parameters.TryGetValue(DialogParameters.SimpleButtons, out object buttons))
            {
                if (buttons is string buttonsValue)
                {
                    if (Enum.TryParse(buttonsValue, out MessageBoxButton msgBoxButton))
                    {
                        dialogButtons = msgBoxButton;
                    }
                }
            }

            if (parameters.TryGetValue(DialogParameters.Icon, out object icon))
            {
                if (icon is string iconValue)
                {
                    if (Enum.TryParse(iconValue, out MessageBoxImage msgBoxImage))
                    {
                        dialogIcon = msgBoxImage;
                    }
                }
            }

            if (parent != null)
            {
                return MessageBoxResultToBoolean(MessageBox.Show(parent, dialogContent, dialogTitle, dialogButtons, dialogIcon));
            }
            else
            {
                return MessageBoxResultToBoolean(MessageBox.Show(dialogContent, dialogTitle, dialogButtons, dialogIcon));
            }
        }

        protected bool? MessageBoxResultToBoolean(MessageBoxResult messageBoxResult)
        {
            switch (messageBoxResult)
            {
                case MessageBoxResult.None:
                    return null;
                case MessageBoxResult.OK:
                    return true;
                case MessageBoxResult.Cancel:
                    return null;
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                default:
                    return null;
            }
        }

        private class SimpleDialog : IDialog
        {
            public bool? DialogResult { get; set; }

            public object DialogValue { get; set; }

            public Dictionary<string, object> Parameters { get; set; }
        }
    }
}
