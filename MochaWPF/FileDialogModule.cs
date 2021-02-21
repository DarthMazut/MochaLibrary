using Microsoft.Win32;
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
    /// Provides a standard implementation of a <see cref="IDialogModule"/> for WPF
    /// <see cref="FileDialog"/> classes. Use this class for handling Open and Save 
    /// dialogs from Win32 namespace.
    /// </summary>
    public sealed class FileDialogModule : IDialogModule
    {
        private FileDialog _dialog;
        private IDialog _backend;
        private Func<IDialogModule, Window> _getParentWindow;
        private bool _isOpen = false;

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

        /// <summary>
        /// Returns new instance of <see cref="FileDialogModule"/>.
        /// </summary>
        /// <param name="dialog">A concrete implementation of <see cref="FileDialog"/> abstract class.</param>
        /// <param name="backend">Provides a backedn data for represented <see cref="FileDialog"/>.</param>
        /// <param name="getParentWindow">
        /// A delegate which returns parent window of this dialog.
        /// This might be called on non-UI-thread; make sure you access application
        /// resources on appropriate thread.
        /// <para>This cannot be null!</para>
        /// </param>
        public FileDialogModule(FileDialog dialog, IDialog backend, Func<IDialogModule, Window> getParentWindow)
        {
            _dialog = dialog;
            _backend = backend;
            _getParentWindow = getParentWindow;
        }

        /// <summary>
        /// This method is not supported for <see cref="FileDialog"/> implementation.
        /// </summary>
        public void Close()
        {
            throw new NotSupportedException("Closing Windows Dialog is currently not supported in MochaLib :(");
        }

        /// <summary>
        /// Sets the backend for represented <see cref="FileDialog"/> instance.
        /// </summary>
        /// <param name="dialog">Backend to be set.</param>
        public void SetDataContext(IDialog backend)
        {
            _backend = backend;
        }

        /// <summary>
        /// This method is not supported for <see cref="FileDialog"/> implementation.
        /// </summary>
        public void Show()
        {
            throw new NotSupportedException("Windows Dialog Module can only be displayed as modal.");
        }

        /// <summary>
        /// This method is not supported for <see cref="FileDialog"/> implementation.
        /// </summary>
        public Task ShowAsync()
        {
            throw new NotSupportedException("Windows Dialog Module can only be displayed as modal.");
        }

        /// <summary>
        /// Displays the dialog. Returns a value representing the outcome of dialog interaction.
        /// </summary>
        public bool? ShowModal()
        {
            Window owner = _getParentWindow?.Invoke(this);

            if (owner != null)
            {
                _isOpen = true;
                bool? result = _dialog.ShowDialog(owner);
                if (result == true)
                {
                    _backend.DialogValue = _dialog.FileName; // !!! Expand here, encapsulate into object !!!
                }
                OnClose();
                return result;
            }
            else
            {
                _isOpen = true;
                bool? result = _dialog.ShowDialog();
                if (result == true)
                {
                    _backend.DialogValue = _dialog.FileName; // !!! Expand here, encapsulate into object !!!
                }
                OnClose();
                return result;
            }
        }

        /// <summary>
        /// Displays the dialog asynchronously. Returns a value representing the outcome of dialog interaction.
        /// </summary>
        public Task<bool?> ShowModalAsync()
        {
            return Task.Run(() =>
            {
                if (_getParentWindow != null)
                {
                    bool? result = null;

                    Window parent = _getParentWindow.Invoke(this);
                    parent.Dispatcher.Invoke(() =>
                    {
                        result = ShowModal();
                    });

                    return result;
                }
                else
                {
                    throw new NotSupportedException("Cannot show dialog async if *getParentWindow* delegate is null.");
                }
            });
        }

        /// <summary>
        /// Marks this dialog as done.
        /// </summary>
        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        private void OnClose()
        {
            _isOpen = false;
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}
