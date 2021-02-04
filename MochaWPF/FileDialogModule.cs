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
    public class FileDialogModule : IDialogModule
    {
        private FileDialog _dialog;
        private IDialog _backend;
        private Func<CommonDialog, Window> _getParentWindow;

        public object View => _dialog;

        public IDialog DataContext => null;

        public bool IsOpen => throw new NotSupportedException();

        public event EventHandler Closed;
        public event EventHandler Disposed;

        public FileDialogModule(FileDialog dialog, IDialog backend, Func<CommonDialog, Window> getParentWindow)
        {
            _dialog = dialog;
            _backend = backend;
            _getParentWindow = getParentWindow;
        }

        public void Close()
        {
            throw new NotSupportedException("Closing Windows Dialog is currently not supported in MochaLib :(");
        }

        public void SetDataContext(IDialog dialog)
        {
            _backend = dialog;
        }

        public void Show()
        {
            throw new NotSupportedException("Windows Dialog Module can only be displayed as modal.");
        }

        public Task ShowAsync()
        {
            throw new NotSupportedException("Windows Dialog Module can only be displayed as modal.");
        }

        public bool? ShowModal()
        {
            Window owner = _getParentWindow?.Invoke(_dialog); 

            if(owner != null)
            {
                bool? result = _dialog.ShowDialog(owner);
                if (result == true)
                {
                    _backend.DialogValue = _dialog.FileName; // !!! Expand here, encapsulate into object !!!
                }
                return result;
            }
            else
            {
                bool? result = _dialog.ShowDialog();
                if (result == true)
                {
                    _backend.DialogValue = _dialog.FileName; // !!! Expand here, encapsulate into object !!!
                }
                return result;
            }
        }

        public Task<bool?> ShowModalAsync()
        {
            return Task.Run(() =>
            {
                if (_getParentWindow != null)
                {
                    bool? result = null;

                    Window parent = _getParentWindow.Invoke(_dialog);
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

        public void Dispose()
        {
            // There is no need of disposing WPF FileDialog.
        }
    }
}
