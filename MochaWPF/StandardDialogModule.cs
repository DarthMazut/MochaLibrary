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
        /// <summary>
        /// A reference to WPF <see cref="System.Windows.Application"/> object.
        /// </summary>
        protected Application Application { get; set; }

        /// <summary>
        /// Determines whether dialog has been disposed.
        /// </summary>
        protected bool IsDisposed { get; set; }

        /// <summary>
        /// Maps results from <see cref="MessageBoxResult"/> to nullable <see langword="bool"/> values.
        /// </summary>
        protected MessageBoxResultMapper Mapper { get; set; }

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

        /// <summary>
        /// Default construtcor. Uses build-in simple implementaion of <see cref="IDialog"/>.
        /// </summary>
        public StandardDialogModule() : this(null, null, null) { }

        /// <summary>
        /// Returns new instance of <see cref="StandardDialogModule"/> class.
        /// </summary>
        /// <param name="dialogData">Backend for represented <see cref="MessageBox"/> dialog.</param>
        public StandardDialogModule(IDialog dialogData) : this(dialogData, null, null) { }

        /// <summary>
        /// Returns new instance of <see cref="StandardDialogModule"/> class.
        /// </summary>
        /// <param name="dialogData">Backend for represented <see cref="MessageBox"/> dialog.</param>
        /// <param name="mapper">Used to map <see cref="MessageBoxResult"/> values to nullable <see langword="bool"/>.</param>
        public StandardDialogModule(IDialog dialogData, MessageBoxResultMapper mapper) : this(dialogData, null, mapper) { }

        /// <summary>
        /// Returns new instance of <see cref="StandardDialogModule"/> class.
        /// </summary>
        /// <param name="dialogData">Backend for represented <see cref="MessageBox"/> dialog.</param>
        /// <param name="application">A reference to WPF <see cref="System.Windows.Application"/> object.</param>
        public StandardDialogModule(IDialog dialogData, Application application) { }

        /// <summary>
        /// Returns new instance of <see cref="StandardDialogModule"/> class.
        /// </summary>
        /// <param name="dialogData">Backend for represented <see cref="MessageBox"/> dialog.</param>
        /// <param name="application">A reference to WPF <see cref="System.Windows.Application"/> object.</param>
        /// <param name="mapper">Used to map <see cref="MessageBoxResult"/> values to nullable <see langword="bool"/>.</param>
        public StandardDialogModule(IDialog dialogData, Application application, MessageBoxResultMapper mapper)
        {
            DataContext = dialogData ?? new SimpleDialogData();
            Application = application;
            Mapper = mapper ?? new MessageBoxResultMapper();
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
                bool? result = this.ShowModal();
                return result;
            });
        }

        /// <summary>
        /// Handles dialog display. Uses <see cref="DialogParameters"/> object to customize
        /// associated dialog and <see cref="MessageBoxResultMapper"/> for result parsing.
        /// </summary>
        protected virtual bool? HandleDialogDisplay()
        {
            DialogParameters parameters = DataContext.DialogParameters;

            if (parameters == null) throw new ArgumentNullException("parameters was null.");

            string title = parameters.Title ?? string.Empty;
            string content = parameters.Message ?? string.Empty;
            MessageBoxButton buttons = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.None;

            if (Enum.TryParse(parameters.PredefinedButtons, out MessageBoxButton msgBoxButton))
            {
                buttons = msgBoxButton;
            }

            if (Enum.TryParse(parameters.Icon, out MessageBoxImage msgBoxImage))
            {
                icon = msgBoxImage;
            }

            Window parent = DialogModuleHelper.GetParentWindow(Application, DataContext);

            if (parent != null)
            {
                return Mapper.MessageBoxResultToBoolean(MessageBox.Show(parent, content, title, buttons, icon));
            }
            else
            {
                return Mapper.MessageBoxResultToBoolean(MessageBox.Show(content, title, buttons, icon));
            }
        }  
    }
}
