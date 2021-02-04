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
    /// Standard implementation of <see cref="IDialogFactory"/> for WPF apps.
    /// Allows for usage of default Windows MessageBox dialogs.
    /// </summary>
    public class StandardDialogFactory : IDialogFactory
    {
        /// <summary>
        /// Returns a <see cref="IDialogModule"/> encapsulating default WPF MessageBox
        /// dialog.
        /// </summary>
        /// <param name="parameters">Allows for customization of returning dialog.</param>
        public IDialogModule Create(string[] parameters)
        {
            if (parameters.Length == 0) throw new ArgumentException("No parameters was given.");
            return new MessageBoxDialog(parameters);
        }

        private class MessageBoxDialog : IDialogModule
        {
            private string[] _parameters;

            public object View => null;

            public IDialog DataContext => null;

            public bool IsOpen => throw new NotImplementedException();

            public event EventHandler Closed;
            public event EventHandler Disposed;

            public MessageBoxDialog(string[] parameters)
            {
                _parameters = parameters;
            }

            public void Close()
            {
                throw new NotSupportedException();
            }

            public void Dispose()
            {
                // No dispose
            }

            public void SetDataContext(IDialog dialog)
            {
                throw new NotSupportedException();
            }

            public void Show()
            {
                throw new NotSupportedException();
            }

            public Task ShowAsync()
            {
                throw new NotSupportedException();
            }

            public bool? ShowModal()
            {
                int paramsLength = _parameters.Length;

                if (paramsLength == 1)
                {
                    // IsOpen = true;
                    MessageBox.Show(_parameters[0]);
                    return true;
                    // Invoke Closed here

                }

                throw new NotImplementedException();
            }

            public Task<bool?> ShowModalAsync()
            {
                throw new NotImplementedException();
            }
        }
    }
}
