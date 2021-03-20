using Microsoft.Win32;
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
    /// Provides a standard implementation of a <see cref="IDialogModule"/> for WPF
    /// <see cref="FileDialog"/> classes. Use this class for handling Open and Save 
    /// dialogs from Win32 namespace.
    /// </summary>
    public sealed class FileDialogModule : IDialogModule
    {
        private FileDialog _dialog;
        private IDialog _backend;
        private Application _application;
        private bool _isOpen = false;
        private bool _isDisposed = false;

        /// <summary>
        /// Returns the underlying <see cref="FileDialog"/> concrete implementation. 
        /// </summary>
        public object View => _dialog;

        /// <summary>
        /// Returns backend data for <see cref="View"/> dialog object.
        /// </summary>
        public IDialog DataContext => _backend;

        /// <summary>
        /// Indicates whether underlying <see cref="FileDialog"/> is currently open.
        /// </summary>
        public bool IsOpen => _isOpen;

        /// <summary>
        /// Fires when represented <see cref="FileDialog"/> is closed.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Fires when this instance is disposed.
        /// </summary>
        public event EventHandler Disposed;

        public event EventHandler<CancelEventArgs> Closing;

        public event EventHandler Opening;

        /// <summary>
        /// Returns new instance of <see cref="FileDialogModule"/>.
        /// </summary>
        /// <param name="application">A reference to WPF <see cref="Application"/> object.</param>
        /// <param name="dialog">A concrete implementation of <see cref="FileDialog"/> abstract class.</param>
        public FileDialogModule(Application application, FileDialog dialog) : this(application, dialog, null) { }

        /// <summary>
        /// Returns new instance of <see cref="FileDialogModule"/>.
        /// </summary>
        /// <param name="application">A reference to WPF <see cref="Application"/> object.</param>
        /// <param name="dialog">A concrete implementation of <see cref="FileDialog"/> abstract class.</param>
        /// <param name="backend">Provides a backedn data for represented <see cref="FileDialog"/>.</param>
        public FileDialogModule(Application application, FileDialog dialog, IDialog backend)
        {
            _application = application;
            _dialog = dialog;
            _backend = backend ?? new SimpleDialogData();
        }

        /// <summary>
        /// This method is not supported for <see cref="FileDialog"/> implementation.
        /// </summary>
        public void Close()
        {
            throw new NotSupportedException("Closing File Dialog is currently not supported in MochaLib :(");
        }

        /// <summary>
        /// Sets the backend for represented <see cref="FileDialog"/> instance.
        /// </summary>
        /// <param name="backend">Backend to be set.</param>
        public void SetDataContext(IDialog backend)
        {
            _backend = backend;
        }

        /// <summary>
        /// This method is not supported for <see cref="FileDialog"/> implementation.
        /// </summary>
        public void Show()
        {
            throw new NotSupportedException("File Dialog Module can only be displayed as modal.");
        }

        /// <summary>
        /// This method is not supported for <see cref="FileDialog"/> implementation.
        /// </summary>
        public Task ShowAsync()
        {
            throw new NotSupportedException("File Dialog Module can only be displayed as modal.");
        }

        /// <summary>
        /// Displays the dialog. Returns a value representing the outcome of dialog interaction.
        /// </summary>
        public bool? ShowModal()
        {
            if (IsOpen)
            {
                throw new InvalidOperationException($"{_dialog.GetType()} was already opened");
            }

            _isOpen = true;

            CustomizeFromParameters();

            Window owner = DialogModuleHelper.GetParentWindow(_application, DataContext);
            bool? result = _dialog.ShowDialog(owner);
            _backend.DialogResult = result;

            if (result == true)
            {
                _backend.DialogValue = _dialog.FileName; // !!! Expand here, encapsulate into object !!!
            }

            OnClose();
            return result;
        }

        /// <summary>
        /// Displays the dialog asynchronously. Returns a value representing the outcome of dialog interaction.
        /// </summary>
        public Task<bool?> ShowModalAsync()
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
        /// Marks this dialog as done.
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

        private void OnClose()
        {
            _isOpen = false;
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void CustomizeFromParameters()
        {
            _dialog.Title = _backend.DialogParameters.Title;
            _dialog.Filter = _backend.DialogParameters.Filter;
            _dialog.DefaultExt = _backend.DialogParameters.DefaultExtension;
            _dialog.InitialDirectory = _backend.DialogParameters.InitialDirectory;
        }
    }
}

