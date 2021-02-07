using Mocha.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MochaWPF
{
    public class StandardDialogModule : IDialogModule
    {
        private IDialog _dataContext;
        private bool _isOpen = false;
        private Func<IDialog, Window> _getParentWindow;

        public object View => null;

        public IDialog DataContext => _dataContext;

        public bool IsOpen => _isOpen;

        public event EventHandler Closed;

        public event EventHandler Disposed;

        public StandardDialogModule() : this(null, null) { }

        public StandardDialogModule(IDialog dialogData) : this(dialogData, null) { }

        public StandardDialogModule(IDialog dialogData, Func<IDialog, Window> getParentWindow)
        {
            _dataContext = dialogData ?? new SimpleDialog();
            _getParentWindow = getParentWindow;
        }

        public void Close()
        {
            throw new NotSupportedException("Closing StandardDialogModule is currently not supported in MochaLib :(");
        }

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public void SetDataContext(IDialog dialogData)
        {
            _dataContext = dialogData;
        }

        public void Show()
        {
            throw new NotSupportedException("StandardDialogModule can only be displayed as modal.");
        }

        public Task ShowAsync()
        {
            throw new NotSupportedException("StandardDialogModule can only be displayed as modal.");
        }

        public bool? ShowModal()
        {
            _isOpen = true;
            bool? result = null;

            if (_getParentWindow != null)
            {
                Window parentWindow = _getParentWindow(_dataContext);
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

        public Task<bool?> ShowModalAsync()
        {
            return Task.Run(() => 
            {
                bool? result = null;

                if (_getParentWindow != null)
                {
                    Window parent = _getParentWindow.Invoke(_dataContext);
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

        protected virtual bool? HandleDialogDisplay(Window parent, string[] parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters was null.");
            if (parameters.Length < 1) throw new ArgumentException("At least one parameter must be passed to display a dialog box.");

            string title = string.Empty;
            string content = string.Empty;
            MessageBoxButton buttons = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.None;

            if(parameters.Length == 1)
            {
                content = parameters[0];

                if(parent != null)
                {
                    return MessageBoxResultToBoolean(MessageBox.Show(parent, content));
                }
                else
                {
                    return MessageBoxResultToBoolean(MessageBox.Show(content));
                }
            }

            if(parameters.Length == 2)
            {
                content = parameters[0];
                title = parameters[1];

                if (parent != null)
                {
                    return MessageBoxResultToBoolean(MessageBox.Show(parent, content, title));
                }
                else
                {
                    return MessageBoxResultToBoolean(MessageBox.Show(content, title));
                }
            }

            if (parameters.Length == 3)
            {
                content = parameters[0];
                title = parameters[1];
                buttons = MessageBoxButton.OK;

                if (Enum.TryParse(parameters[2], out MessageBoxButton msgBoxButton))
                {
                    buttons = msgBoxButton;
                }

                if (parent != null)
                {
                    return MessageBoxResultToBoolean(MessageBox.Show(parent, content, title, buttons));
                }
                else
                {
                    return MessageBoxResultToBoolean(MessageBox.Show(content, title, buttons));
                }
            }

            if (parameters.Length > 3)
            {
                content = parameters[0];
                title = parameters[1];
                buttons = MessageBoxButton.OK;
                icon = MessageBoxImage.None;

                if (Enum.TryParse(parameters[2], out MessageBoxButton msgBoxButton))
                {
                    buttons = msgBoxButton;
                }

                if (Enum.TryParse(parameters[3], out MessageBoxImage msgBoxIcon))
                {
                    icon = msgBoxIcon;
                }

                if (parent != null)
                {
                    return MessageBoxResultToBoolean(MessageBox.Show(parent, content, title, buttons, icon));
                }
                else
                {
                    return MessageBoxResultToBoolean(MessageBox.Show(content, title, buttons, icon));
                }
            }

            throw new Exception("It appears that StandardDialogModule:HandleDialogDisplay() is not exhaustive...");
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
                    return true;
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return true;
                default:
                    return null;
            }
        }

        private class SimpleDialog : IDialog
        {
            public bool? DialogResult { get; set; }

            public object DialogValue { get; set; }

            public string[] Parameters { get; set; }
        }
    }
}
