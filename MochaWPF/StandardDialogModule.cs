using Mocha.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// <summary>
        /// A reference to WPF <see cref="System.Windows.Application"/> object.
        /// </summary>
        protected Application Application { get; set; }

        /// <summary>
        /// Determines whether dialog has been disposed.
        /// </summary>
        protected bool IsDisposed { get; set; }

        /// <summary>
        /// There is no view object which represents WPF <see cref="MessageBox"/> dialog
        /// therefore this always returns <see langword="null"/>.
        /// </summary>
        public object View => null;

        /// <summary>
        /// Returns an <see cref="IDialog"/> object which serve as a backend
        /// for represented <see cref="MessageBox"/>. 
        /// </summary>
        public IDialog DataContext { get; protected set; }

        /// <summary>
        /// Indicates whether the represented <see cref="MessageBox"/> is currently open.
        /// </summary>
        public bool IsOpen { get; protected set; }

        /// <summary>
        /// Fires when represented <see cref="MessageBox"/> closes.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Fires when this instance is done.
        /// </summary>
        public event EventHandler Disposed;

        public event EventHandler<CancelEventArgs> Closing;

        public event EventHandler Opening;

        /// <summary>
        /// Returns new instance of <see cref="StandardDialogModule"/> class.
        /// </summary>
        /// <param name="application">A reference to WPF <see cref="System.Windows.Application"/> object.</param>
        public StandardDialogModule(Application application) : this(application, null) { }

        /// <summary>
        /// Returns new instance of <see cref="StandardDialogModule"/> class.
        /// </summary>
        /// <param name="application">A reference to WPF <see cref="System.Windows.Application"/> object.</param>
        /// <param name="dialogData">Backend for represented <see cref="MessageBox"/> dialog.</param>
        public StandardDialogModule(Application application, IDialog dialogData)
        {
            Application = application;
            DataContext = dialogData ?? new SimpleDialogData();
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
        /// This method is not supported by <see cref="StandardDialogModule"/> implementation.
        /// </summary>
        public virtual void Close()
        {
            throw new NotSupportedException("Closing StandardDialogModule is currently not supported in MochaLib");
        }

        /// <summary>
        /// Marks this dialog as done.
        /// </summary>
        public virtual void Dispose()
        {
            if(IsDisposed)
            {
                return;
            }
            else
            {
                OnDisposed();
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Sets backend data for this dialog.
        /// </summary>
        /// <param name="dialogData">Backend data to be set.</param>
        public virtual void SetDataContext(IDialog dialogData)
        {
            DataContext = dialogData;
        }

        /// <summary>
        /// Showing <see cref="MessageBox"/> in non-modal way is not supported by this implementation.
        /// </summary>
        public virtual void Show()
        {
            throw new NotSupportedException("StandardDialogModule can only be displayed as modal.");
        }

        /// <summary>
        /// Showing <see cref="MessageBox"/> asynchronously in non-modal way is not supported by this implementation.
        /// </summary>
        public virtual Task ShowAsync()
        {
            throw new NotSupportedException("StandardDialogModule can only be displayed as modal.");
        }

        /// <summary>
        /// Displays represented <see cref="MessageBox"/> dialog in modal manner.
        /// Returns result of dialog interaction.
        /// </summary>
        public virtual bool? ShowModal()
        {
            if (IsOpen)
            {
                throw new InvalidOperationException($"{GetType()} was already opened");
            }

            if(IsDisposed)
            {
                throw new InvalidOperationException("Cannot show already disposed dialog");
            }

            IsOpen = true;

            bool? result = HandleDialogDisplay();

            IsOpen = false;
            OnClose();

            DataContext.DialogResult = result;
            return result;
        }

        /// <summary>
        /// Displays asynchronously represented <see cref="MessageBox"/> dialog in modal manner.
        /// Returns result of dialog interaction.
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
        /// Handles dialog display. Uses <see cref="DialogParameters"/> object to customize
        /// associated dialog.
        /// </summary>
        protected virtual bool? HandleDialogDisplay()
        {
            DialogParameters parameters = DataContext.DialogParameters;

            if (parameters == null) throw new ArgumentNullException("parameters are null.");

            string title = parameters.Title ?? string.Empty;
            string content = parameters.Message ?? string.Empty;
            MessageBoxButton buttons = ResolveButtons(parameters.PredefinedButtons);
            MessageBoxImage icon = ResolveIcon(parameters.Icon);

            Window parent = DialogModuleHelper.GetParentWindow(Application, DataContext);
            return ResolveResult(MessageBox.Show(parent, content, title, buttons, icon));
        }

        /// <summary>
        /// Maps <see cref="DialogParameters.PredefinedButtons"/> string into <see cref="MessageBoxButton"/> value.
        /// </summary>
        /// <param name="predefinedButtons">Value to be mapped.</param>
        protected virtual MessageBoxButton ResolveButtons(string predefinedButtons)
        {
            if (Enum.TryParse(predefinedButtons, out MessageBoxButton msgBoxButton))
            {
                return msgBoxButton;
            }

            return MessageBoxButton.OK;
        }

        /// <summary>
        /// Maps <see cref="DialogParameters.Icon"/> string into <see cref="MessageBoxImage"/> value.
        /// </summary>
        /// <param name="icon">Value to be mapped.</param>
        protected virtual MessageBoxImage ResolveIcon(string icon)
        {
            if (Enum.TryParse(icon, out MessageBoxImage msgBoxImage))
            {
                return msgBoxImage;
            }

            return MessageBoxImage.None;
        }

        /// <summary>
        /// Maps results from <see cref="MessageBoxResult"/> to nullable <see langword="bool"/> values.
        /// </summary>
        /// <param name="result">Value to be mapped.</param>
        protected virtual bool? ResolveResult(MessageBoxResult result)
        {
            switch (result)
            {
                case MessageBoxResult.None:
                    return null;
                case MessageBoxResult.OK:
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                default:
                    return null;
            }
        }
    }
}
